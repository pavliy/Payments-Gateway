namespace Domain.Core;

public abstract class ItemNotFoundException : Exception
{
    protected ItemNotFoundException()
    {
    }

    protected ItemNotFoundException(string message, Guid itemId)
        : base(message)
    {
        this.ItemId = itemId;
    }

    public Guid ItemId { get; init; }
}