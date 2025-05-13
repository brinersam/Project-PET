using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.VolunteerRequests.Contracts.Requests;
public record CreateVolunteerRequestRequest(
    VolunteerAccountDto AccountDto,
    Guid UserId)
{ }
