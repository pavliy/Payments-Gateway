using ApplicationServices.PaymentsManagement.Dto;
using ApplicationServices.PaymentsManagement.Validation;

using FluentValidation.TestHelper;

using NUnit.Framework;

namespace ApplicationServices.Tests.PaymentsManagement.Validation;

public class CardDetailsWithSecureValidatorTests
{
    private CardDetailsWithSecureValidator target = null!;

    [SetUp]
    public void BeforeEach()
    {
        this.target = new CardDetailsWithSecureValidator(new CardDetailsValidator());
    }

    [TestCase(1)]
    [TestCase(3333)]
    public void WhenCvvCodeIsNotValid_ShouldFailWithError(int wrongCvvCode)
    {
        var card = new CardDetailsWithSecure("1111-3333-4444-5555", 12, 24, wrongCvvCode);
        this.target.TestValidate(card).ShouldHaveValidationErrorFor(i => i.CvvCode);
    }

    [TestCase(101)]
    [TestCase(930)]
    public void WhenCvvCodeIsValid_ShouldPassValidation(int validCvvCode)
    {
        var card = new CardDetailsWithSecure("1111-3333-4444-5555", 12, 24, validCvvCode);
        this.target.TestValidate(card).ShouldNotHaveValidationErrorFor(i => i.CvvCode);
    }
    
    [TestCase("1-1111-1111-1111")]
    [TestCase("5168-30744-822-0999")]
    public void WhenCardNumberIsNotValid_ShouldFailWithError(string wrongCardNumber)
    {
        var card = new CardDetailsWithSecure(wrongCardNumber, 12, 23, 999);
        this.target.TestValidate(card).ShouldHaveValidationErrorFor(i => i.Number);
    }
}