using CKO.BankOfUk.API.Models;

using Microsoft.AspNetCore.Mvc;

namespace CKO.BankOfUk.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BankOfUkController : ControllerBase
{
    private readonly Serilog.ILogger logger;

    public BankOfUkController(Serilog.ILogger logger)
    {
        this.logger = logger;
    }
    
    [HttpPost(Name = "post")]
    public ActionResult Post([FromBody] OperationInfo operationBody)
    {
        this.logger.Information("Processing card {CardNumber}", operationBody.CardNumber);
        
        var randomGenerator = new Random();

        int randomNumber = randomGenerator.Next(10, 1000);
        return randomNumber switch
            {
                > 400 and < 500 => throw new Exception("Something weird happened"),
                > 500 and < 600 => new ConflictResult(),
                _ => new OkResult()
            };
    }
}