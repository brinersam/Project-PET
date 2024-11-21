using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Contracts.Requests;
public record CreatePetRequest(
    string Name,
    string Coat,
    string Description,
    DateTime DateOfBirth,
    AnimalDataDto AnimalData,
    HealthInfoDto HealthInfo,
    List<PaymentInfoDto> PaymentInfos,
    AddressDto Address,
    PhonenumberDto Phonenumber,
    PetStatusDto Status = PetStatusDto.NotSet);
