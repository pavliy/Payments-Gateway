using System.Text.RegularExpressions;

using Domain.Core;

namespace Domain.Payments;

// public record CardDetails(string Number, int ExpireMonth, int ExpireYear);
public class Card : ValueObject
{
    private static readonly Regex MaskNumberRegex = new("[0-9](?=.*.{4})", RegexOptions.Compiled);

    public Card(string number, int expireMonth, int expireYear)
    {
        this.Number = number;
        this.ExpireMonth = expireMonth;
        this.ExpireYear = expireYear;
    }

    public string Number { get; private set; }

    public int ExpireMonth { get; init; }

    public int ExpireYear { get; init; }

    public void MaskNumber()
    {
        this.Number = Card.MaskNumberRegex.Replace(this.Number, "X");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Number;
        yield return this.ExpireMonth;
        yield return this.ExpireYear;
    }
}