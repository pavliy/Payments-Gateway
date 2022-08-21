using ApplicationServices.PaymentsManagement.Dto;

using FluentValidation;

namespace ApplicationServices.PaymentsManagement.Validation;

public class CardDetailsWithSecureValidator : AbstractValidator<CardDetailsWithSecure>
{
    public CardDetailsWithSecureValidator(CardDetailsValidator cardDetailsValidator)
    {
        this.Include(cardDetailsValidator);
        this.RuleFor(i => i.CvvCode).GreaterThan(99).LessThan(1000);
    }
}