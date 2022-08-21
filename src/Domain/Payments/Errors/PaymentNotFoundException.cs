using Domain.Core;

namespace Domain.Payments.Errors;

public class PaymentNotFoundException : ItemNotFoundException
{
    public PaymentNotFoundException(Guid itemId)
        : base($"Payment '{itemId}' was not found", itemId)
    {
    }
}