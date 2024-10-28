namespace ProjectPet.Application.UseCases.Volunteers;

public record UpdateVolunteerInfoRequest(
    Guid Id,
    UpdateVolunteerInfoRequestDto Dto)
{ }
