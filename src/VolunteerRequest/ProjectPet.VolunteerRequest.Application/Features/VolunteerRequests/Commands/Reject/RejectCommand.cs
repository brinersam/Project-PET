using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Reject;
public record RejectCommand(
    string RejectionComment,
    Guid RequestId,
    string PermissionCode) : IMapFromRequest<RejectCommand, RejectVolunteerRequestRequest, Guid, string>
{
    public static RejectCommand FromRequest(RejectVolunteerRequestRequest request, Guid requestId, string permissionCode)
        => new(request.RejectionComment, requestId, permissionCode);
}
