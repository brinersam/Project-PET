using ProjectPet.Core.Requests;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.RequestRevision;
public record RequestRevisionCommand(
    string RevisionComment,
    Guid RequestId) : IMapFromRequest<RequestRevisionCommand, RequestRevisionVolunteerRequestRequest, Guid>
{
    public static RequestRevisionCommand FromRequest(RequestRevisionVolunteerRequestRequest request, Guid requestId)
        => new(request.RevisionComment, requestId);
}
