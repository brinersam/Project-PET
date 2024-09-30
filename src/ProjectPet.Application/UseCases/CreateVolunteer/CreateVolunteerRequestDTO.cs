using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public record CreateVolunteerRequestDto(
        string FullName,
        string Email,
        string Description,
        int YOExperience,
        PhoneNumberDto Phonenumber,
        IEnumerable<Pet> OwnedPets,
        IEnumerable<PaymentInfo>? PaymentMethods,
        IEnumerable<SocialNetwork>? SocialNetworks)
    { }

    public record PhoneNumberDto(
        string Phonenumber,
        string PhonenumberAreaCode)
    { }

}
