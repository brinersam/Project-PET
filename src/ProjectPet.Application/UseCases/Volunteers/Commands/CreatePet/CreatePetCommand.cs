using ProjectPet.Application.Dto;
using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.CreatePet;

public record CreatePetCommand(
    Guid Id,
    string Name,
    string Coat,
    string Description,
    DateOnly DateOfBirth,
    Guid AnimalData_SpeciesId,
    string AnimalData_BreedName,
    HealthInfoDto HealthInfo,
    List<PaymentInfoDto> PaymentInfos,
    AddressDto Address,
    PhoneNumberDto PhoneNumber,
    PetStatus Status = PetStatus.NotSet);
