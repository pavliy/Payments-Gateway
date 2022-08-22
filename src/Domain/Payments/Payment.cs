using Domain.Core;

namespace Domain.Payments;

public class Payment : Entity, IAggregationRoot
{
    public Payment(Card card, Expense spent)
    {
        this.Card = card ?? throw new ArgumentNullException(nameof(card));
        this.Spent = spent ?? throw new ArgumentNullException(nameof(spent));
    }

    public Expense Spent { get; init; }

    public Card Card { get; init; }

    public PaymentStatus Status { get; private set; }

    public void Approve()
    {
        this.Status = PaymentStatus.Approved;
    }

    public void Fail()
    {
        this.Status = PaymentStatus.Failed;
    }
}