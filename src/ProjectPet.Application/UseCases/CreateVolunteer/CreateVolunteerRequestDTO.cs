using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public record CreateVolunteerRequestDto(
    string FullName,
    string Email,
    string Description,
    int YOExperience,
    string Phonenumber,
    string PhonenumberAreaCode,
    IEnumerable<Pet> OwnedPets,
    IEnumerable<PaymentInfo>? PaymentMethods,
    IEnumerable<SocialNetwork>? SocialNetworks)
    { }

}
