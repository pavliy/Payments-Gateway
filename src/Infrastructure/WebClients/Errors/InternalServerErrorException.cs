using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.WebClients.Errors;

public class InternalServerErrorException : Exception
{
    public InternalServerErrorException(string errorMessage)
        : base(errorMessage)
    {
    }

    public ProblemDetails? ProblemDetails { get; set; }
}