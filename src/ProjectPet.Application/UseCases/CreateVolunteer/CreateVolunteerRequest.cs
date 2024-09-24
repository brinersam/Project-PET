using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public record CreateVolunteerRequest
    {
        public CreateVolunteerRequest(string fullName, string email, string description, int yOExperience, string phonenumber, string phonenumberAreaCode, IEnumerable<Pet> ownedPets, IEnumerable<PaymentInfo>? paymentMethods, IEnumerable<SocialNetwork>? socialNetworks)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            YOExperience = yOExperience;
            Phonenumber = phonenumber;
            PhonenumberAreaCode = phonenumberAreaCode;
            OwnedPets = ownedPets;
            PaymentMethods = paymentMethods;
            SocialNetworks = socialNetworks;
        }

        public string FullName { get; } = null!;
        public string Email {get; } = null!;
        public string Description {get; } = null!;
        public int YOExperience { get; }
        public string Phonenumber { get; } = null!;
        public string PhonenumberAreaCode { get; } = null!;
        public IEnumerable<Pet> OwnedPets { get; } = null!;
        public IEnumerable<PaymentInfo>? PaymentMethods { get; } = null!;
        public IEnumerable<SocialNetwork>? SocialNetworks { get; } = null!;

    }
}
