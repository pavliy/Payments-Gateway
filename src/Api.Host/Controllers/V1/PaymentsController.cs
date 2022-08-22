using System.Net;

using Api.Host.ErrorHandling;

using ApplicationServices.PaymentsManagement;
using ApplicationServices.PaymentsManagement.Create;
using ApplicationServices.PaymentsManagement.Dto;
using ApplicationServices.PaymentsManagement.Retrieve;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.ChmGen;

namespace Api.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[RetryPolicyExceptionFilter]
public class PaymentsController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly Serilog.ILogger logger;

    public PaymentsController(IMediator mediator, Serilog.ILogger logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "create_payment", Tags = new[] { "Payments Gateway" })]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<CreatedResult> CreatePayment([FromBody] CreatePaymentCommand createPaymentCommand)
    {
        Guid paymentId = await this.mediator.Send(createPaymentCommand);
        return new CreatedResult($"/v1/payments/{paymentId}", paymentId);
    }

    [HttpGet]
    [SwaggerOperation(OperationId = "get_payment", Tags = new[] { "Payments Gateway" })]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<PaymentDetails> GetPayment([FromQuery] Guid paymentId)
    {
        PaymentDetails result = await this.mediator.Send(new GetPaymentQuery(paymentId));
        return result;
    }
}