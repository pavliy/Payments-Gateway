using System.Diagnostics.CodeAnalysis;

using ApplicationServices;
using ApplicationServices.Common;

using FluentValidation;
using FluentValidation.AspNetCore;

namespace Api.Host.Extensions;

[ExcludeFromCodeCoverage]
public static class ValidationExtensions
{
    public static IMvcCoreBuilder AddFluentValidationRegistrations(this IMvcCoreBuilder builder)
    {
        builder.Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<ApplicationServicesAssembly>(
                filter:
                scanResult => !typeof(IChildValidator).IsAssignableFrom(scanResult.ValidatorType));

        return builder;
    }
}