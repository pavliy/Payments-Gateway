namespace Domain.Payments.Errors;

public sealed class InvalidCardNumberException : Exception
{
    public InvalidCardNumberException(string message)
        : base(message)
    {
    }
}