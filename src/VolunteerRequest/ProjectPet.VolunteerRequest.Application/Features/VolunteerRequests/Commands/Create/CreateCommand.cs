using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Create;
public record CreateCommand(
    VolunteerAccountDto VolunteerAccountDto,
    Guid UserId) : IMapFromRequest<CreateCommand, CreateVolunteerRequestRequest, Guid>
{
    public static CreateCommand FromRequest(CreateVolunteerRequestRequest request, Guid userId)
        => new CreateCommand(request.AccountDto, userId);
}
