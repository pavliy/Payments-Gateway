using Domain.Core;
using Domain.Payments;

namespace Infrastructure.Persistence;

public class DataRepository<T> : IDataRepository<Payment>
    where T : Entity
{
    private readonly PaymentsDbContext paymentsDbContext;

    public DataRepository(PaymentsDbContext paymentsDbContext)
    {
        this.paymentsDbContext = paymentsDbContext;
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await this.paymentsDbContext.SaveEntitiesAsync(cancellationToken);
    }

    public void Add(Payment payment)
    {
        this.paymentsDbContext.AddPayment(payment);
    }

    public Task<Payment?> FindAsync(Guid paymentId, CancellationToken cancellationToken, bool returnCopy = true)
    {
        Payment? payment = this.paymentsDbContext.FindPayment(paymentId, returnCopy);
        return Task.FromResult(payment);
    }
}