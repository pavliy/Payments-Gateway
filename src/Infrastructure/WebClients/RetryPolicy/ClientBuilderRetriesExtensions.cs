using System.Diagnostics.CodeAnalysis;
using System.Net;

using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using Polly.Timeout;

using Serilog;

namespace Infrastructure.WebClients.RetryPolicy;

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1408:ConditionalExpressionsMustDeclarePrecedence")]
public static class ClientBuilderRetriesExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IHttpClientBuilder AddDefaultPolicies(
        this IHttpClientBuilder clientBuilder,
        RetryPolicyOptions retryOptions)
    {
        return clientBuilder.AddDefaultRetryPolicy(retryOptions).AddDefaultCircuitBreakerPolicy(retryOptions)
            .AddTimeoutPolicy(retryOptions);
    }

    private static IHttpClientBuilder AddDefaultCircuitBreakerPolicy(
        this IHttpClientBuilder clientBuilder,
        RetryPolicyOptions retryOptions)
    {
        return clientBuilder.AddPolicyHandler(
            ClientBuilderRetriesExtensions.BuildCircuitBreakerPolicy(retryOptions));
    }

    private static IHttpClientBuilder AddDefaultRetryPolicy(
        this IHttpClientBuilder clientBuilder,
        RetryPolicyOptions retryOptions)
    {
        IEnumerable<TimeSpan>? delay = Backoff.DecorrelatedJitterBackoffV2(
            TimeSpan.FromSeconds(1),
            retryOptions.RetriesCount);
        return clientBuilder.AddPolicyHandler(
            Policy
                .HandleResult<HttpResponseMessage>(
                    r => (int)r.StatusCode >= 500 && r.StatusCode != HttpStatusCode.InsufficientStorage
                         || r.StatusCode == HttpStatusCode.RequestTimeout)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    delay,
                    (response, _, retryCount, _) =>
                        {
                            Log.Warning(
                                "Attempt #{RetryCount} to request {RetryUrl} after last response with code {RetryResponseCode}",
                                retryCount,
                                response.Result.RequestMessage?.RequestUri,
                                response.Result.StatusCode);
                        }));
    }

    private static IHttpClientBuilder AddTimeoutPolicy(
        this IHttpClientBuilder httpClientBuilder,
        RetryPolicyOptions retryOptions)
    {
        return httpClientBuilder.AddPolicyHandler(
            Policy.TimeoutAsync<HttpResponseMessage>(retryOptions.RetryTimeout));
    }

    private static AsyncCircuitBreakerPolicy<HttpResponseMessage> BuildCircuitBreakerPolicy(
        RetryPolicyOptions retryOptions)
    {
        return Policy
            .HandleResult<HttpResponseMessage>(
                r => (int)r.StatusCode >= 500 && r.StatusCode != HttpStatusCode.InsufficientStorage
                     || r.StatusCode is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests)
            .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                retryOptions.CircuitBreakThreshold,
                retryOptions.BreakDuration,
                (response, _, _, _) =>
                    Log.Warning(
                        "Circuit break {CircuitUrl} after response code {RetryResponseCode}",
                        response.Result.RequestMessage?.RequestUri,
                        response.Result.StatusCode),
                _ => { },
                () => { });
    }
}