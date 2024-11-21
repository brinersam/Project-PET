using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdateVolunteerInfo;

public record UpdateVolunteerInfoCommand(
    Guid Id,
    CreateVolunteerNullableDto Dto) : IMapFromRequest<UpdateVolunteerInfoCommand, UpdateVolunteerInfoRequest, Guid>
{
    public static UpdateVolunteerInfoCommand FromRequest(UpdateVolunteerInfoRequest req, Guid id)
    {
        return new UpdateVolunteerInfoCommand(id, req.dto);
    }
}
