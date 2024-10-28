namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerInfo;

public record UpdateVolunteerInfoRequest(
    Guid Id,
    UpdateVolunteerInfoRequestDto Dto)
{ }
