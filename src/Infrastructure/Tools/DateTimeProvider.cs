namespace Infrastructure.Tools;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcDate => DateTime.UtcNow;
}