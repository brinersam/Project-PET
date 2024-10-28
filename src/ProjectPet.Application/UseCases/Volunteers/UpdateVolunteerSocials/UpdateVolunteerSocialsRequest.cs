namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerSocials;

public record UpdateVolunteerSocialsRequest(
    Guid Id,
    UpdateVolunteerSocialsRequestDto Dto)
{ }
