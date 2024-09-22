namespace ProjectPet.Domain.Models
{
    public record PetPhoto
    {
        public string StoragePath { get; private set; } = null!;
        public bool IsPrimary { get; private set; }
    }
}
