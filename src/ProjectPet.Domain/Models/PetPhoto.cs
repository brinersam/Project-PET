namespace ProjectPet.Domain.Models
{
    public class PetPhoto
    {
        public Guid Id { get; private set; }
        public string Species { get; private set; } = null!;
        public bool IsPrimary { get; private set; }
    }
}
