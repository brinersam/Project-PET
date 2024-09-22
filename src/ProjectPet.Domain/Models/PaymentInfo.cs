namespace ProjectPet.Domain.Models
{
    public record PaymentInfo
    {
        public string Title { get; private set; } = null!;
        public string Instructions { get; private set; } = null!;
    }
}

