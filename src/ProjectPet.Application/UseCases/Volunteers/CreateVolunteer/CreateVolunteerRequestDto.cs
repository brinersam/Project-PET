using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.Volunteers.CreateVolunteer;

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
