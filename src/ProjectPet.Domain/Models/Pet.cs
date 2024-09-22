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

        //private List<PaymentInfo> _paymentMethods = [];
        //private List<PetPhoto> _photos = [];
        //public IReadOnlyList<PaymentInfo> PaymentMethods => _paymentMethods;
        //public IReadOnlyList<PetPhoto> Photos => _photos;

        public PhotoList? Photos { get; private set; }
        public PaymentMethodsList? PaymentMethods { get; private set; }

        //public void AddPaymentMethod(PaymentInfo paymentInfo)
        //{
        //    //_paymentMethods.Add(paymentInfo);
        //}
        //public void AddPetPhoto(PetPhoto petPhoto)
        //{
        //    //_photos.Add(petPhoto);
        //}

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

