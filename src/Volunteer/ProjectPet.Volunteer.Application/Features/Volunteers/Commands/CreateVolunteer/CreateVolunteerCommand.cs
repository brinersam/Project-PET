
using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreateVolunteer;

public record CreateVolunteerCommand(
    CreateVolunteerDto VolunteerDto) : IMapFromRequest<CreateVolunteerCommand, CreateVolunteerRequest>
{
    public static CreateVolunteerCommand FromRequest(CreateVolunteerRequest req)
    {
        return new CreateVolunteerCommand(req.VolunteerDto);
    }
}
