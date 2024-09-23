namespace ProjectPet.Domain.Models
{
    public record SocialNetwork
    {
        public string Name { get; private set; } = null!;
        public string Link { get; private set; } = null!;
    }
}
