using ApplicationServices.Events;
using ApplicationServices.PaymentsManagement.Dto;

using Domain.Core;
using Domain.Payments;

using MediatR;

namespace ApplicationServices.PaymentsManagement.Create;

public sealed class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, Guid>
{
    private readonly IEventsDispatcherService eventDispatcher;

    private readonly IDataRepository<Payment> paymentsDataRepository;

    public CreatePaymentHandler(
        IDataRepository<Payment> paymentsDataRepository,
        IEventsDispatcherService eventDispatcher)
    {
        this.paymentsDataRepository = paymentsDataRepository;
        this.eventDispatcher = eventDispatcher;
    }

    public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        (CardDetails? cardDetails, ExpenseDetails? expenseDetails) = request;
        var card = new Card(cardDetails.Number, cardDetails.ExpireMonth, cardDetails.ExpireYear);
        var expense = new Expense(expenseDetails.Amount, expenseDetails.Currency);

        var payment = new Payment(card, expense);
        this.paymentsDataRepository.Add(payment);
        await this.paymentsDataRepository.SaveAsync(cancellationToken);

        await this.eventDispatcher.DispatchItemAsync(
            new PaymentCreatedEvent(payment.Uuid, request.Card.CvvCode),
            cancellationToken);

        return payment.Uuid;
    }
}