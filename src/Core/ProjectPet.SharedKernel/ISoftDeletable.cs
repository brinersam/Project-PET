namespace ProjectPet.SharedKernel;

public interface ISoftDeletable
{
    void SoftDelete();
    void SoftRestore();
}
