using ProjectPet.Domain.Models.DDD;

namespace ProjectPet.Domain.Models
{
    public class Pet : Entity
    {
        public string Name { get; private set; } = null!;
        public AnimalData AnimalData { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string Coat { get; private set; } = null!;
        public HealthInfo HealthInfo { get; private set; } = null!;
        public Address Address { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;
        public Status Status { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public DateOnly CreatedOn { get; private set; }
        public PhotoList? Photos { get; private set; }
        public PaymentMethodsList? PaymentMethods { get; private set; }
        public Pet(Guid id) : base(id)
        {
        }
    }

    public record PaymentMethodsList
    {
        public List<PaymentInfo> Data { get; private set; }
    }

    public record PhotoList
    {
        public List<PetPhoto> Data { get; private set; }
    }

    public enum Status
    {
        NotSet,
        Requires_Care,
        Looking_For_Home,
        Home_Found
    }
}

