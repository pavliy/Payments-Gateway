using ApplicationServices.PaymentsManagement.Dto;

using FluentValidation;

namespace ApplicationServices.PaymentsManagement.Validation;

public class PaymentDetailsValidator : AbstractValidator<PaymentDetails>
{
    public PaymentDetailsValidator()
    {
        this.ClassLevelCascadeMode = CascadeMode.Stop;
        this.RuleLevelCascadeMode = CascadeMode.Stop;

        this.RuleFor(p => p.Card).SetValidator(new CardDetailsValidator());
        this.RuleFor(p => p.Spent).SetValidator(new ExpenseDetailsValidator());
    }
}