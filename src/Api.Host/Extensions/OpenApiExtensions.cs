using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace Api.Host.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning(
            options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.RegisterMiddleware = true;
                });

        services.AddVersionedApiExplorer(
            options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });

        services.AddSwaggerGen(
            options =>
                {
                    options.EnableAnnotations();
                    options.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                            {
                                Version = "0.1.0",
                                Title = "Payments Gateway API",
                                Description = "<h2>Intermediate between merchant and bank</h2>",
                                Extensions = new Dictionary<string, IOpenApiExtension>
                                    {
                                        { "termsOfService", new OpenApiString("github.com/pavliy") },
                                        { "licence", new OpenApiObject { { "name", new OpenApiString("pavliy") } } },
                                        {
                                            "contact", new OpenApiObject
                                                {
                                                    { "name", new OpenApiString("Eugene Pavliy") },
                                                    {
                                                        "email", new OpenApiString(
                                                            "fake_sample_x@gmail.com")
                                                    }
                                                }
                                        }
                                    }
                            });

                    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    options.DescribeAllParametersInCamelCase();
                });

        return services;
    }
}