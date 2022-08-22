namespace Domain.Payments.Errors;

public sealed class InvalidExpireYearException : Exception
{
    public InvalidExpireYearException(string message)
        : base(message)
    {
    }
}