using System.Text.RegularExpressions;

using Domain.Core;
using Domain.Payments.Errors;

namespace Domain.Payments;

// public record CardDetails(string Number, int ExpireMonth, int ExpireYear);
public class Card : ValueObject
{
    private static readonly Regex MaskNumberRegex = new("[0-9](?=.*.{4})", RegexOptions.Compiled);

    private static readonly Regex ValidateNumberRegex = new(@"\d{4}-?\d{4}-?\d{4}-?\d{4}", RegexOptions.Compiled);

    public Card(string number, int expireMonth, int expireYear)
    {
        if (!Card.ValidateNumberRegex.IsMatch(number))
        {
            throw new InvalidCardNumberException($"Invalid card number specified: {number}");
        }

        this.Number = number;

        if (expireMonth is < 1 or > 12)
        {
            throw new InvalidExpireMonthException($"Specified month is not correct: {expireMonth}");
        }

        this.ExpireMonth = expireMonth;

        if (expireYear is < 2022 or > 2050)
        {
            throw new InvalidExpireYearException($"Specified year is not correct: {expireYear}");
        }

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