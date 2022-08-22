namespace Domain.Payments.Errors;

public sealed class InvalidExpireMonthException : Exception
{
    public InvalidExpireMonthException(string message)
        : base(message)
    {
    }
}