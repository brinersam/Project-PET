using ProjectPet.Domain.Models.DDD;

namespace ProjectPet.Domain.Models
{
    public class Pet : Entity
    {
        public string Name { get; private set; } = null!;
        public string Species { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string Breed { get; private set; } = null!;
        public string Coat { get; private set; } = null!;
        public string Health { get; private set; } = null!;
        public string Address { get; private set; } = null!;
        public float Weight { get; private set; }
        public float Height { get; private set; }
        public string PhoneNumber { get; private set; } = null!;
        public bool IsSterilized { get; private set; }
        public bool IsVaccinated { get; private set; }
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

