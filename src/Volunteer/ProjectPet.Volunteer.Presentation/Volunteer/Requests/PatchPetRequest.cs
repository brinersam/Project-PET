using ProjectPet.Application.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.PatchPet;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

public record PatchPetRequest(
    string? Name,
    AnimalDataDto? AnimalData,
    string? Description,
    string? Coat,
    HealthInfoDto? HealthInfo,
    AddressDto? Address,
    PhonenumberDto? Phonenumber)
    : IToCommand<PatchPetCommand, Guid, Guid>
{
    public PatchPetCommand ToCommand(Guid volunteerId, Guid petid)
        => new(
            volunteerId,
            petid,
            Name,
            AnimalData,
            Description,
            Coat,
            HealthInfo,
            Address,
            Phonenumber);
}
