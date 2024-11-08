using ProjectPet.Application.Dto;
using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.Volunteers.CreatePet;
public record CreatePetRequest(
    string Name,
    string Coat,
    string Description,
    DateTime DateOfBirth,
    Guid AnimalData_SpeciesId,
    string AnimalData_BreedName,
    HealthInfoDto HealthInfo,
    List<PaymentInfoDto> PaymentInfos,
    AddressDto Address,
    PhoneNumberDto PhoneNumber,
    Status Status = Status.NotSet);
