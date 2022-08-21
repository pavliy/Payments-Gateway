using ApplicationServices.PaymentsManagement.Dto;

using FluentValidation;

namespace ApplicationServices.PaymentsManagement.Validation;

public class ExpenseDetailsValidator : AbstractValidator<ExpenseDetails>, IChildValidator
{
    public ExpenseDetailsValidator()
    {
        this.RuleLevelCascadeMode = CascadeMode.Stop;
        this.ClassLevelCascadeMode = CascadeMode.Stop;

        this.RuleFor(e => e.Amount).GreaterThan(0).LessThan(10000);
        this.RuleFor(e => e.Currency).NotEmpty().NotNull().Matches("USD|EUR|GBP");
    }
}