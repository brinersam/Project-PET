using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Contracts.Requests;
namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Update;
public record UpdateCommand(
    Guid RequestId,
    VolunteerAccountDto VolunteerAccountDto) : IMapFromRequest<UpdateCommand, UpdateVolunteerRequestRequest, Guid>
{
    public static UpdateCommand FromRequest(UpdateVolunteerRequestRequest request, Guid requestId)
        => new(requestId, request.VolunteerAccountDto);
}
