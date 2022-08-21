namespace Infrastructure.Tools;

public interface ICorrelationInfoAccessor
{
    string TraceId { get; init; }
}