using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models.DDD;
using ProjectPet.Domain.Shared;
namespace ProjectPet.Domain.Models
{
    public class Volunteer : EntityBase
    {
        public string FullName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public int YOExperience { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; } = null!;
        private List<Pet> _ownedPets;
        public IReadOnlyList<Pet> OwnedPets => _ownedPets;
        public PaymentMethodsList? PaymentMethods { get; private set; }
        public SocialNetworkList? SocialNetworks { get; private set; }

        public Volunteer(Guid id) : base(id) { }

        private Volunteer(
            Guid id,
            string fullName,
            string email,
            string description,
            int yOExperience,
            PhoneNumber phoneNumber,
            IEnumerable<Pet> ownedPets,
            IEnumerable<PaymentInfo> paymentMethods,
            IEnumerable<SocialNetwork> socialNetworks) : base(id)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            YOExperience = yOExperience;
            PhoneNumber = phoneNumber;
            _ownedPets = ownedPets.ToList();
            PaymentMethods = new() { Data = paymentMethods.ToList() };
            SocialNetworks = new() { Data = socialNetworks.ToList() };
        }

        public static Result<Volunteer,Error> Create
            (
            Guid id,
            string fullName,
            string email,
            string description,
            int yOExperience,
            PhoneNumber phoneNumber,
            IEnumerable<Pet> ownedPets,
            IEnumerable<PaymentInfo> paymentMethods,
            IEnumerable<SocialNetwork> socialNetworks)
        {
            if (id.Equals(Guid.Empty))
                return Errors.General.ValueIsEmptyOrNull(id,nameof(id));

            if (String.IsNullOrWhiteSpace(fullName))
                return Errors.General.ValueIsEmptyOrNull(fullName,nameof(fullName));

            if (String.IsNullOrWhiteSpace(email))
                return Errors.General.ValueIsEmptyOrNull(email, nameof(email));

            if (String.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsEmptyOrNull(description, nameof(description));

            return new Volunteer
                (
                    id,
                    fullName,
                    email,
                    description,
                    yOExperience,
                    phoneNumber,
                    ownedPets,
                    paymentMethods,
                    socialNetworks
                );
        }

        public int PetsHoused() => _ownedPets.Count(x => x.Status == Status.Home_Found);
        public int PetsLookingForHome() => _ownedPets.Count(x => x.Status == Status.Looking_For_Home);
        public int PetsInCare() => _ownedPets.Count(x => x.Status == Status.Requires_Care);
    }
    public record SocialNetworkList
    {
        public List<SocialNetwork> Data { get; set; }
    }
}
