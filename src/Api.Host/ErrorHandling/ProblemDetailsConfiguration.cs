using System.Diagnostics.CodeAnalysis;

using Domain.Core;

using Hellang.Middleware.ProblemDetails;

using Microsoft.AspNetCore.Mvc;

namespace Api.Host.ErrorHandling;

[ExcludeFromCodeCoverage]
public static class ProblemDetailsConfiguration
{
    public static IServiceCollection AddTunedProblemDetails(
        this IServiceCollection services,
        IHostEnvironment hostEnvironment)
    {
        services.AddProblemDetails(
            setup =>
                {
                    setup.IncludeExceptionDetails = (ctx, env) =>
                        hostEnvironment.IsDevelopment() || hostEnvironment.IsStaging();

                    setup.Map<ItemNotFoundException>(
                        _ => new ProblemDetails
                            {
                                Title = "Not found",
                                Status = StatusCodes.Status404NotFound,
                                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
                            });
                });
        return services;
    }
}