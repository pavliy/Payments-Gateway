using ApplicationServices.PaymentsManagement.Dto;
using ApplicationServices.PaymentsManagement.Validation;

using FluentValidation.TestHelper;

using NUnit.Framework;

namespace ApplicationServices.Tests.PaymentsManagement.Validation;

public class ExpenseDetailsValidatorTests
{
    private ExpenseDetailsValidator target = null!;

    private const string ValidCurrency = "USD";

    private const decimal ValidAmount = 500;
    
    [SetUp]
    public void BeforeEach()
    {
        this.target = new ExpenseDetailsValidator();
    }
    
    [TestCase(0)]
    [TestCase(-100)]
    [TestCase(10000)]
    public void WhenAmountIsLessThen0OrExceedsMax_ShouldFailWithError(decimal amount)
    {
        var expense = new ExpenseDetails(amount, ExpenseDetailsValidatorTests.ValidCurrency);
        this.target.TestValidate(expense).ShouldHaveValidationErrorFor(e => e.Amount);
    }

    [Test]
    public void WhenAmountIsPositiveNumber_ShouldNotFail()
    {
        var expense = new ExpenseDetails(120, ExpenseDetailsValidatorTests.ValidCurrency);
        this.target.TestValidate(expense).ShouldNotHaveValidationErrorFor(e => e.Amount);
    }
    
    [TestCase("UAH")]
    [TestCase("RUB")]
    [TestCase("CHN")]
    public void WhenCurrencyIsNotFromAllowedList_ShouldFailWithError(string notAllowedCurrency)
    {
        var expense = new ExpenseDetails(ExpenseDetailsValidatorTests.ValidAmount, notAllowedCurrency);
        this.target.TestValidate(expense).ShouldHaveValidationErrorFor(e => e.Currency);
    }
    
    [TestCase("USD")]
    [TestCase("GBP")]
    [TestCase("EUR")]
    public void WhenCurrencyFromAllowedList_ShouldFailWithError(string allowedCurrency)
    {
        var expense = new ExpenseDetails(ExpenseDetailsValidatorTests.ValidAmount, allowedCurrency);
        this.target.TestValidate(expense).ShouldNotHaveValidationErrorFor(e => e.Currency);
    }
}