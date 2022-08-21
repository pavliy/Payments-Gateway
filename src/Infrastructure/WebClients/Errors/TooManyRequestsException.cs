namespace Infrastructure.WebClients.Errors;

public class TooManyRequestsException : Exception
{
    public TooManyRequestsException(string message)
        : base(message)
    {
    }
}