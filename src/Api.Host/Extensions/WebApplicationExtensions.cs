using Microsoft.OpenApi.Models;

namespace Api.Host.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseTunedSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(
            c =>
                {
                    c.PreSerializeFilters.Add(
                        (swaggerDoc, httpReq) =>
                            {
                                swaggerDoc.Servers = new List<OpenApiServer>
                                    {
                                        new
                                            () { Url = "/v1" }
                                    };

                                var paths =
                                    new OpenApiPaths();
                                foreach ((string? pathKey, OpenApiPathItem? pathValue) in swaggerDoc.Paths)
                                {
                                    paths.Add(
                                        pathKey.Replace("v1/", string.Empty, StringComparison.InvariantCulture),
                                        pathValue);
                                }

                                swaggerDoc.Paths = paths;
                            });
                });
        app.UseSwaggerUI(
            options =>
                {
                    options.ConfigObject.ShowCommonExtensions = true;
                    options.ConfigObject.TryItOutEnabled = true;
                    options.SwaggerEndpoint("/swagger/v1/swagger.yaml", "V1");
                });

        return app;
    }
}