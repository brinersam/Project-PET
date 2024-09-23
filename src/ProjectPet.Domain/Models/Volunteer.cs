using ProjectPet.Domain.Models.DDD;

namespace ProjectPet.Domain.Models
{
    public class Volunteer : Entity
    {
        public string FullName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public int YOExperience { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; } = null!;
        private List<Pet> _ownedPets = [];
        public IReadOnlyList<Pet> OwnedPets => _ownedPets;
        public PaymentMethodsList? PaymentMethods { get; private set; }
        public SocialNetworkList? SocialNetworks { get; private set; }

        public Volunteer(Guid id) : base(id)
        {
        }

        public int PetsHoused() => _ownedPets.Count(x => x.Status == Status.Home_Found);
        public int PetsLookingForHome() => _ownedPets.Count(x => x.Status == Status.Looking_For_Home);
        public int PetsInCare() => _ownedPets.Count(x => x.Status == Status.Requires_Care);
    }
    public record SocialNetworkList
    {
        public List<SocialNetwork> Data { get; private set; } = [];
    }
}
