using ProjectPet.Core.Requests;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.PatchPet;

public record PatchPetCommand(
    Guid VolunteerId,
    Guid Petid,
    string? Name,
    AnimalDataDto? AnimalData,
    string? Description,
    string? Coat,
    HealthInfoDto? HealthInfo,
    AddressDto? Address,
    PhonenumberDto? Phonenumber) : IMapFromRequest<PatchPetCommand, PatchPetRequest, Guid, Guid>
{
    public static PatchPetCommand FromRequest(PatchPetRequest request, Guid volunteerId, Guid petid)
    {
        return new(
            volunteerId,
            petid,
            request.Name,
            request.AnimalData,
            request.Description,
            request.Coat,
            request.HealthInfo,
            request.Address,
            request.Phonenumber);
    }
}
