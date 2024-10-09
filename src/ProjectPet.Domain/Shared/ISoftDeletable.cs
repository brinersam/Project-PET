namespace ProjectPet.Domain.Shared
{
    public interface ISoftDeletable
    {
        void SetIsDeletedFlag(bool value);
    }
}
