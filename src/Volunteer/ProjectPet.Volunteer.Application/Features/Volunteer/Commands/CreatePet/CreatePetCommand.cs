using ProjectPet.Application.Dto;
using ProjectPet.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.CreatePet;

public record CreatePetCommand(
    Guid Id,
    string Name,
    string Coat,
    string Description,
    DateOnly DateOfBirth,
    AnimalDataDto AnimalData,
    HealthInfoDto HealthInfo,
    List<PaymentInfoDto> PaymentInfos,
    AddressDto Address,
    PhonenumberDto PhoneNumber,
    PetStatus Status = PetStatus.NotSet);
