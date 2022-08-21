using System.Diagnostics.CodeAnalysis;

using Autofac;

using Infrastructure;

namespace Api.Host.Extensions;

[ExcludeFromCodeCoverage]
public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureAutofacContainer(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureContainer<ContainerBuilder>(
            (_, builder) =>
                {
                    // builder.RegisterLogger();
                    builder.RegisterAssemblyModules(typeof(InfrastructureAssembly).Assembly);
                });
        return hostBuilder;
    }
}