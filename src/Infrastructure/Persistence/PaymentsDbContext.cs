using Domain.Payments;

using JsonNet.ContractResolvers;

using MediatR;

using Newtonsoft.Json;

namespace Infrastructure.Persistence;

public class PaymentsDbContext
{
    private readonly IMediator mediator;

    // In EF CORE we will typically use DBSet.
    // But in this demo I need to copy object on get to emulate real state and work with immutability in mind.
    private readonly Dictionary<Guid, Payment> payments = new();

    public PaymentsDbContext(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public void AddPayment(Payment payment)
    {
        this.payments.Add(payment.Uuid, payment);
    }

    public Payment? FindPayment(Guid paymentId, bool returnCopy)
    {
        this.payments.TryGetValue(paymentId, out Payment? payment);

        if (payment == null)
        {
            return null;
        }

        if (!returnCopy)
        {
            return payment;
        }

        // Again, it's done only for demo. Because we don't have isolated context and items are shared in memory
        var settings = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };
        string serialized = JsonConvert.SerializeObject(payment);
        return JsonConvert.DeserializeObject<Payment>(serialized, settings);
    }

#pragma warning disable CA1822
    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken)
#pragma warning restore CA1822
    {
        // That usually looks like this
        // await this.SaveChangesAsync(cancellationToken)
        return Task.FromResult(true);
    }
}