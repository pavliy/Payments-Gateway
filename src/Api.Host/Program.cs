using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Threading.Channels;

using Api.Host.ErrorHandling;
using Api.Host.Extensions;
using Api.Host.Infrastructure.Configuration;

using ApplicationServices.Events;

using Autofac.Extensions.DependencyInjection;

using Hellang.Middleware.ProblemDetails;

using Infrastructure.WebClients;
using Infrastructure.WebClients.RetryPolicy;

using Microsoft.AspNetCore.Mvc;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

using Worker.Emulation;

try
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(new JsonFormatter())
        .CreateBootstrapLogger();

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    ConfigureHost(builder.Host);

    await using WebApplication webApplication = builder.Build();
    ConfigureApplication(webApplication);
    await webApplication.RunAsync();
    return 0;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

static void ConfigureHost(IHostBuilder hostBuilder)
{
    hostBuilder.UseServiceProviderFactory(_ => new AutofacServiceProviderFactory());

    hostBuilder.ConfigureServices(ConfigureServices);
    hostBuilder.ConfigureAutofacContainer();

    hostBuilder.UseSerilog(
        (context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services));
}

#pragma warning disable CS8321
static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
#pragma warning restore CS8321
{
    services.AddLogging();
    services.AddTunedProblemDetails(context.HostingEnvironment);
    services.AddMvcCore()
        .AddFluentValidationRegistrations();

    services
        .AddControllers()
        .AddJsonOptions(
            options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter());
                });

    services.AddRouting(options => options.LowercaseUrls = true);

    services
        .AddHealthChecks();

    services.AddSwagger();
    services.RegisterOptions(context.Configuration);

    var retryOptions = context.Configuration.GetSection(RetryPolicyOptions.SettingsSectionName)
        .Get<RetryPolicyOptions>();
    services.AddHttpClients(retryOptions);
    services.AddApiVersioning(
        options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

    services.AddVersionedApiExplorer(
        options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    services.AddHostedService<PaymentsProcessorHostedService>();

    // Register channel as demo alternative to distributed queue.
    // In real world we would go with RabbitMQ / Service Bus whatever
    services.AddSingleton(Channel.CreateUnbounded<IntegrationEvent>(new UnboundedChannelOptions()));
    services.AddSingleton(svc => svc.GetRequiredService<Channel<IntegrationEvent>>().Reader);
    services.AddSingleton(svc => svc.GetRequiredService<Channel<IntegrationEvent>>().Writer);
}

static void ConfigureApplication(WebApplication app)
{
    if (!app.Environment.IsProduction())
    {
        app.UseTunedSwagger();
    }

    if (app.Environment.IsProduction())
    {
        app.UseHttpsRedirection();
    }

    app.UseProblemDetails();
    app.UseSerilogRequestLogging();
    app.MapControllers();
    app.MapHealthChecks("/healthz");

    app.Lifetime.ApplicationStopped.Register(() => { Log.Logger.Debug("Application has stopped"); });
}

[ExcludeFromCodeCoverage]
#pragma warning disable SA1601
public partial class Program
#pragma warning restore SA1601
{
}