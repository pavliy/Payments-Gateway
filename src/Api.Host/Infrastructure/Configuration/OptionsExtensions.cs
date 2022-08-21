using Infrastructure.Configuration;
using Infrastructure.WebClients.RetryPolicy;

namespace Api.Host.Infrastructure.Configuration;

public static class OptionsExtensions
{
    public static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<BankOfUkConfiguration>()
            .Bind(configuration.GetSection(BankOfUkConfiguration.SettingsSectionName));

        services.AddOptions<RetryPolicyOptions>()
            .Bind(configuration.GetSection(RetryPolicyOptions.SettingsSectionName));

        return services;
    }
}