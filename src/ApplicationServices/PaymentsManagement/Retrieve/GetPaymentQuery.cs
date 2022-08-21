using ApplicationServices.PaymentsManagement.Dto;

using MediatR;

namespace ApplicationServices.PaymentsManagement.Retrieve;

public record GetPaymentQuery(Guid PaymentId) : IRequest<PaymentDetails>;