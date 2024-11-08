using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerInfo;

public record UpdateVolunteerInfoRequestDto(
    string? FullName,
    string? Email,
    string? Description,
    int? YOExperience,
    PhoneNumberDto? PhoneNumber)
{ }
