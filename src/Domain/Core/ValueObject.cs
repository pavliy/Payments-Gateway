namespace Domain.Core;

public abstract class ValueObject
{
    public static bool operator ==(ValueObject? a, ValueObject? b)
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

    public static bool operator !=(ValueObject? a, ValueObject? b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (this.GetType() != obj.GetType())
        {
            return false;
        }

        var valueObject = (ValueObject)obj;

        return this.GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return this.GetEqualityComponents()
            .Aggregate(
                1,
                (current, obj) =>
                    {
                        unchecked
                        {
                            return (current * 23) + obj.GetHashCode();
                        }
                    });
    }

    protected abstract IEnumerable<object> GetEqualityComponents();
}