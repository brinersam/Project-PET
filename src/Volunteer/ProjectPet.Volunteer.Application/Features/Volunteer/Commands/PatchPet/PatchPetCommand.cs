using ProjectPet.Application.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.PatchPet;

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
