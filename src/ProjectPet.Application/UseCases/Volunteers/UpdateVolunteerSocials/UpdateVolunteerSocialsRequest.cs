namespace ProjectPet.Application.UseCases.Volunteers
{
    public record UpdateVolunteerSocialsRequest(
        Guid Id,
        UpdateVolunteerSocialsRequestDto Dto)
    { }

}
