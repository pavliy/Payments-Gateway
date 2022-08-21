using ApplicationServices.PaymentsManagement.Dto;

using MediatR;

namespace ApplicationServices.PaymentsManagement.Create;

public record CreatePaymentCommand(CardDetailsWithSecure Card, ExpenseDetails Expense) : IRequest<Guid>;