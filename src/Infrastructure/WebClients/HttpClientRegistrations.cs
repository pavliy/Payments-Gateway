using System.Diagnostics.CodeAnalysis;

using Infrastructure.Configuration;
using Infrastructure.WebClients.RetryPolicy;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.WebClients;

[ExcludeFromCodeCoverage]
public static class HttpClientRegistrations
{
    public const string BankOfUkHttpClient = nameof(HttpClientRegistrations.BankOfUkHttpClient);

    public static IServiceCollection AddHttpClients(
        this IServiceCollection services,
        RetryPolicyOptions retryPolicyOptions)
    {
        services.AddHttpClient(
                HttpClientRegistrations.BankOfUkHttpClient,
                (serviceProvider, client) =>
                    {
                        BankOfUkConfiguration? options = serviceProvider
                            .GetRequiredService<IOptions<BankOfUkConfiguration>>().Value;
                        client.BaseAddress = new Uri(options.Url);
                    })
            .AddDefaultPolicies(retryPolicyOptions);
        return services;
    }
}