using System.Text.RegularExpressions;

using ApplicationServices.PaymentsManagement.Dto;

using FluentValidation;

namespace ApplicationServices.PaymentsManagement.Validation;

public class CardDetailsValidator : AbstractValidator<CardDetails>, IChildValidator
{
    public CardDetailsValidator()
    {
        this.RuleFor(c => c.Number)
            .NotNull()
            .NotEmpty()
            .Matches(
                @"\d{4}-?\d{4}-?\d{4}-?\d{4}",
                RegexOptions.Compiled);

        this.RuleFor(c => c.ExpireMonth).GreaterThanOrEqualTo(1).LessThanOrEqualTo(12);
        this.RuleFor(c => c.ExpireYear).GreaterThanOrEqualTo(2022).LessThan(2050);
    }
}