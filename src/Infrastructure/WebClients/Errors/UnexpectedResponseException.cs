namespace Infrastructure.WebClients.Errors;

public class UnexpectedResponseException : Exception
{
    public UnexpectedResponseException(string errorMessage)
        : base(errorMessage)
    {
    }
}