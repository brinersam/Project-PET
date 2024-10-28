namespace ProjectPet.Application.UseCases.Volunteers;

public record UpdateVolunteerInfoRequestDto(
    string? FullName,
    string? Email,
    string? Description,
    int? YOExperience,
    PhoneNumberDto? PhoneNumber)
{ }
