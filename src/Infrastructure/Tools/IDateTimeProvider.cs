namespace Infrastructure.Tools;

public interface IDateTimeProvider
{
    public DateTime UtcDate { get; }
}