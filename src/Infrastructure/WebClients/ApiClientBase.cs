using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

using Infrastructure.Serialization;
using Infrastructure.WebClients.Errors;

using Microsoft.AspNetCore.Mvc;

using Serilog;

namespace Infrastructure.WebClients;

public abstract class ApiClientBase
{
    protected readonly IJsonSerializer JsonSerializer;

    private readonly HttpClient client;

    protected ApiClientBase(
        IJsonSerializer jsonSerializer,
        HttpClient client)
    {
        this.JsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    protected async Task<TResponse?> SendInternal<TResponse>(
        string urlPath,
        object? message,
        HttpMethod httpMethod,
        CancellationToken cancellationToken)
    {
        Log.Debug("Prepare request ({MessageType}). Url: {UrlPath}", message?.GetType().Name, urlPath);

        string? requestBody = null;

        var requestMessage = new HttpRequestMessage(httpMethod, urlPath);
        if (message != null)
        {
            requestBody = this.JsonSerializer.Serialize(message);
            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            requestMessage.Content = content;
        }

        requestMessage.Headers.Add("Accept", "application/json");

        var stopwatch = Stopwatch.StartNew();

        Log.Debug("Sending request: {RequestMessage}", requestBody);

        HttpResponseMessage responseMessage = await this.client
            .SendAsync(requestMessage, cancellationToken);

        Log.Debug(
            "Sent ({StatusCode} {ReasonPhrase}) - {ContentLength}, Elapsed: {ElapsedMilliseconds} ms",
            (int)responseMessage.StatusCode,
            responseMessage.ReasonPhrase,
            responseMessage.Content.Headers.ContentLength,
            stopwatch.ElapsedMilliseconds);

        // ReSharper disable once SwitchStatementMissingSomeCases
        switch (responseMessage.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    try
                    {
                        return await this.FindResponseData<TResponse>(responseMessage);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Failed to parse returned json data. Will switch to string representation");
                        string receivedMessage = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                        throw new UnexpectedResponseException(receivedMessage);
                    }
                }

            case HttpStatusCode.InternalServerError:
                {
                    ProblemDetails? problemDetails = null;
                    try
                    {
                        problemDetails = await this.FindResponseData<ProblemDetails>(responseMessage);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex, "Failed to get problem details during internal server error parse");
                    }

                    throw new InternalServerErrorException("Internal server error")
                        {
                            ProblemDetails = problemDetails
                        };
                }

            case (HttpStatusCode)429:
                {
                    throw new TooManyRequestsException("Too many requests");
                }

            case HttpStatusCode.Unauthorized:
                {
                    throw new UnauthorizedRequestException();
                }

            default:
                {
                    object? responseData = null;
                    try
                    {
                        responseData = await this.FindResponseData<object?>(responseMessage);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex, "Failed to get response as json. Will try as string");
                        if (responseMessage.Content.Headers.ContentLength > 0)
                        {
                            responseData = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                        }
                    }

                    Log
                        .ForContext("ReasonPhrase", responseMessage.ReasonPhrase)
                        .ForContext("RequestMessage", requestBody)
                        .ForContext("ResponseMessage", responseData)
                        .ForContext("StatusCode", (int)responseMessage.StatusCode)
                        .Error("Http error during request to API");

                    throw new UnexpectedResponseException("Error response status code during request to the API.");
                }
        }
    }

    protected Task<TResponse?> SendInternal<TResponse>(
        string urlPath,
        HttpMethod httpMethod,
        CancellationToken cancellationToken)
    {
        return this.SendInternal<TResponse?>(urlPath, null, httpMethod, cancellationToken);
    }

    private async Task<T?> FindResponseData<T>(HttpResponseMessage responseMessage)
    {
        T? responseData = default;
        if (responseMessage.Content.Headers.ContentLength is null or 0)
        {
            return responseData;
        }

        Stream errorResponseStream = await responseMessage.Content.ReadAsStreamAsync();
        responseData = this.JsonSerializer.Deserialize<T?>(errorResponseStream)!;

        return responseData;
    }
}