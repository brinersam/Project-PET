namespace ProjectPet.Domain.Models
{
    public class Volunteer
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public int YOExperience { get; private set; }
        public string PhoneNumber { get; private set; } = null!;
        private List<Pet> _ownedPets = [];
        //private List<SocialNetwork> _socialNetworks = [];
        //private List<PaymentInfo> _paymentMethods = [];
        public int PetsHoused => _ownedPets.Count(x => x.Status == Status.Home_Found);
        public int PetsLookingForHome => _ownedPets.Count(x => x.Status == Status.Looking_For_Home);
        public int PetsInCare => _ownedPets.Count(x => x.Status == Status.Requires_Care);
        public IReadOnlyList<Pet> OwnedPets => _ownedPets;
        //public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
        //public IReadOnlyList<PaymentInfo> PaymentMethods => _paymentMethods;

        public PaymentMethodsList? PaymentMethods { get; private set; }
        public SocialNetworkList? SocialNetworks { get; private set; }
    }
    public record SocialNetworkList
    {
        public List<SocialNetwork> Data { get; private set; }
    }
}
