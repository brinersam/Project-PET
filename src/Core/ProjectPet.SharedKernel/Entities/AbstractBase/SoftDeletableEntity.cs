namespace ProjectPet.SharedKernel.Entities.AbstractBase;
public abstract class SoftDeletableEntity : EntityBase, ISoftDeletable
{
    public bool IsDeleted { get; private set; }
    public DateTime DeletionDate { get; private set; }

    protected SoftDeletableEntity(Guid id) : base(id)
    {}

    public virtual void SoftDelete()
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletionDate = DateTime.UtcNow;
    }

    public virtual void SoftRestore()
    {
        IsDeleted = false;
        DeletionDate = default;
    }
}
