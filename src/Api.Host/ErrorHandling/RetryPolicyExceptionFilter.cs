using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Polly.CircuitBreaker;
using Polly.Timeout;

using Serilog;

namespace Api.Host.ErrorHandling;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RetryPolicyExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        StatusCodeResult? codeResult = context.Exception switch
            {
                BrokenCircuitException => new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable),
                TimeoutRejectedException => new StatusCodeResult((int)HttpStatusCode.RequestTimeout),
                _ => null
            };

        if (codeResult == null)
        {
            return;
        }

        Log.Warning(context.Exception, "Failure due to retry policy");
        context.ExceptionHandled = true;
        context.Result = codeResult;
    }
}