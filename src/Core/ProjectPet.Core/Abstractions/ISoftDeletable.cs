namespace ProjectPet.Core.Abstractions;

public interface ISoftDeletable
{
    void Delete();
    void Restore();
}
