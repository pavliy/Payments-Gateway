using Domain.Core;

namespace Domain.Payments;

public class Expense : ValueObject
{
    public Expense(decimal amount, string currency)
    {
        this.Amount = amount;
        this.Currency = currency;
    }

    public decimal Amount { get; init; }

    public string Currency { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Amount;
        yield return this.Currency;
    }
}