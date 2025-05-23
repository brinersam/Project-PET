using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.VolunteerRequests.Contracts.Requests;
public record UpdateVolunteerRequestRequest(
    VolunteerAccountDto VolunteerAccountDto)
{ }
