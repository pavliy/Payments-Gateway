using ApplicationServices.PaymentsManagement.Dto;
using ApplicationServices.PaymentsManagement.Validation;

using FluentValidation.TestHelper;

using NUnit.Framework;

namespace ApplicationServices.Tests.PaymentsManagement.Validation;

public class CardDetailsValidatorTests
{
    private CardDetailsValidator target = null!;

    private const int ValidMonth = 12;

    private const int ValidYear = 23;
    
    [SetUp]
    public void BeforeEach()
    {
        this.target = new CardDetailsValidator();
    }
    
    [TestCase("1-1111-1111-1111")]
    [TestCase("5168-30744-822-0999")]
    public void WhenCardNumberIsNotValid_ShouldFailWithError(string wrongCardNumber)
    {
        var card = new CardDetails(wrongCardNumber, CardDetailsValidatorTests.ValidMonth, CardDetailsValidatorTests.ValidYear);
        this.target.TestValidate(card).ShouldHaveValidationErrorFor(i => i.Number);
    }

    [TestCase("5169-3075-0482-9669")]
    [TestCase("5169208415159012")]
    public void WhenCardNumberIsValid_ShouldNotFailWithError(string validCardNumber)
    {
        var card = new CardDetails(validCardNumber, CardDetailsValidatorTests.ValidMonth, CardDetailsValidatorTests.ValidYear);
        this.target.TestValidate(card).ShouldNotHaveValidationErrorFor(i => i.Number);
    }
}