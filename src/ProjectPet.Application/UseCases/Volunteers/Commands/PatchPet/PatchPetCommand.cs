using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.PatchPet;

public record PatchPetCommand(
    Guid VolunteerId,
    Guid Petid,
    string? Name,
    AnimalDataDto? AnimalData,
    string? Description,
    string? Coat,
    HealthInfoDto? HealthInfo,
    AddressDto? Address,
    PhonenumberDto? Phonenumber);
