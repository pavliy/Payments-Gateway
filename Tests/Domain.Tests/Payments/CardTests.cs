using Domain.Payments;

using FluentAssertions;

using NUnit.Framework;

namespace Domain.Tests.Payments;

public class CardTests
{
    [Test]
    [TestCase("1111222233334444", "XXXXXXXXXXXX4444")]
    [TestCase("5167-3333-5555-9999", "XXXX-XXXX-XXXX-9999")]
    public void MaskNumber_ShouldMaskCardNumber(string cardNumber, string maskedCardNumber)
    {
        var card = new Card(cardNumber, 12, 22);
        card.MaskNumber();
        card.Number.Should().Be(maskedCardNumber);
    }
}