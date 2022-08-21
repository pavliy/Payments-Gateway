namespace Domain.Core;

public interface IDataRepository<T>
    where T : Entity
{
    void Add(T item);

    Task<T?> FindAsync(Guid paymentEntity, CancellationToken cancellationToken, bool returnCopy = true);

    Task SaveAsync(CancellationToken cancellationToken);
}