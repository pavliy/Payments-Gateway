using System.Diagnostics;

namespace Infrastructure.Tools;

public class SampleCorrelationInfoAccessor : ICorrelationInfoAccessor
{
    // NOTE: this is sample version
    // Typically this goes from systems like Datadog or Appinsights.
    // It also can be fully custom through the whole pipe - really DEPENDS on our ECOSYSTEM
    public string TraceId { get; init; } = Activity.Current?.Id ?? Guid.NewGuid().ToString("N");
}