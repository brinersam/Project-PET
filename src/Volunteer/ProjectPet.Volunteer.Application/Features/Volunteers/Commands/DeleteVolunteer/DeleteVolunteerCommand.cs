using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeleteVolunteer;

public record DeleteVolunteerCommand(Guid Id, bool SoftDelete = true)
    : IMapFromRequest<DeleteVolunteerCommand, DeleteVolunteerRequest>
{
    public static DeleteVolunteerCommand FromRequest(DeleteVolunteerRequest request)
        => new DeleteVolunteerCommand(request.Id, request.SoftDelete);
}
