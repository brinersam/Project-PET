using ProjectPet.API.Etc;
using ProjectPet.Application.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.PatchPet;

namespace ProjectPet.API.Requests.Volunteers;

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
