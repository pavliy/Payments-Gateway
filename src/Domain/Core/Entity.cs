namespace Domain.Core;

public abstract class Entity
{
    protected internal Entity()
        : this(Guid.NewGuid(), 0)
    {
    }

    private Entity(Guid uuid, long id)
    {
        this.Uuid = uuid;
        this.Id = id;
    }

    public long Id { get; init; }

    public Guid Uuid { get; init; }

    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
        {
            return false;
        }

        if (object.ReferenceEquals(this, other))
        {
            return true;
        }

        if (this.GetType() != other.GetType())
        {
            return false;
        }

        if (this.Id == 0 || other.Id == 0)
        {
            return false;
        }

        return this.Id == other.Id;
    }

    public override int GetHashCode()
    {
        // If the Id will change, it will be another entity
        return (this.GetType().ToString() + this.Id).GetHashCode();
    }
}