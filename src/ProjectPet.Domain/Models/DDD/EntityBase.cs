namespace ProjectPet.Domain.Models.DDD;

public abstract class EntityBase
{
    public Guid Id { get; private set; } = Guid.Empty;
    protected EntityBase(Guid id)
    {
        Id = id;
    }
    public override bool Equals(object? obj)
    {
        if (obj is not EntityBase other)
            return false;

        if (ReferenceEquals(this, other) == false)
            return false;

        if (Id.Equals(default) || other.Id.Equals(default))
            return false;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return (GetType().FullName + Id).GetHashCode();
    }

    public static bool operator ==(EntityBase a, EntityBase b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }
    public static bool operator !=(EntityBase a, EntityBase b)
    {
        return !(a == b);
    }
}
