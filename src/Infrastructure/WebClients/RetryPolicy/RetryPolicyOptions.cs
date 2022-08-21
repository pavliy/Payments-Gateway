using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.WebClients.RetryPolicy;

[ExcludeFromCodeCoverage]

// ReSharper disable once UnusedType.Global
public class RetryPolicyOptions
{
    public const string SettingsSectionName = "RetryPolicy";

    public int RetriesCount { get; init; } = 3;

    public int CircuitBreakThreshold { get; init; } = 15;

    public TimeSpan BreakDuration { get; init; } = TimeSpan.FromSeconds(30);

    public TimeSpan RetryTimeout { get; init; } = TimeSpan.FromSeconds(10);

    public TimeSpan ClientTimeout { get; init; } = TimeSpan.FromSeconds(15);
}