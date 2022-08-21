using ApplicationServices.PaymentsManagement.Dto;

using AutoMapper;

using Domain.Core;
using Domain.Payments;
using Domain.Payments.Errors;

using MediatR;

namespace ApplicationServices.PaymentsManagement.Retrieve;

public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, PaymentDetails>
{
    private readonly IDataRepository<Payment> paymentsRepository;

    private readonly IMapper mapper;

    public GetPaymentHandler(IDataRepository<Payment> paymentsRepository, IMapper mapper)
    {
        this.paymentsRepository = paymentsRepository;
        this.mapper = mapper;
    }

    public async Task<PaymentDetails> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
    {
        Payment? payment = await this.paymentsRepository.FindAsync(request.PaymentId, cancellationToken);
        if (payment == null)
        {
            throw new PaymentNotFoundException(request.PaymentId);
        }

        payment.Card.MaskNumber();
        return this.mapper.Map<PaymentDetails>(payment);
    }
}