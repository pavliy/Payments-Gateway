using ApplicationServices.PaymentsManagement.Dto;
using ApplicationServices.PaymentsManagement.Retrieve;

using AutoMapper;

using Domain.Core;
using Domain.Payments;
using Domain.Payments.Errors;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace ApplicationServices.Tests.PaymentsManagement.Retrieve;

public class GetPaymentHandlerTests
{
    private static readonly Guid FakeId = Guid.Parse("446a182d-5327-431a-89da-c85a2e0935cc");

    private readonly GetPaymentQuery
        fakeQuery = new(GetPaymentHandlerTests.FakeId);

    private Mock<IMapper> mapperStub = null!;

    private Mock<IDataRepository<Payment>> paymentsRepositoryStub = null!;

    private GetPaymentHandler target = null!;

    [SetUp]
    public void BeforeEach()
    {
        this.paymentsRepositoryStub = new Mock<IDataRepository<Payment>>();
        this.mapperStub = new Mock<IMapper>();
        this.target = new GetPaymentHandler(this.paymentsRepositoryStub.Object, this.mapperStub.Object);
    }

    [Test]
    [Ignore("Should verify correctly")]
    public async Task WhenItemFoundInRepository_ShouldReturnItWithMaskedNumber()
    {
        var fakePayment = new Payment(new Card("5169-2075-8430-2120", 12, 2022), new Expense(100, "USD"));
        this.paymentsRepositoryStub
            .Setup(pr => pr.FindAsync(GetPaymentHandlerTests.FakeId, CancellationToken.None, true))
            .ReturnsAsync(fakePayment);

        PaymentDetails receivedPayment = await this.target.Handle(this.fakeQuery, CancellationToken.None);
        receivedPayment.Card.Number.Should().Be("X");
    }

    [Test]
    public void WhenItemNotFoundInRepository_ShouldThrowNotFoundException()
    {
        Func<Task<PaymentDetails>> action = () => this.target.Handle(this.fakeQuery, CancellationToken.None);
        action.Should().ThrowAsync<PaymentNotFoundException>();
    }
}