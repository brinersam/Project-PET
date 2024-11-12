namespace ProjectPet.Application.Dto;

public record VolunteerDto(
    string FullName,
    string Email,
    string Description,
    int YOExperience,
    PhoneNumberDto Phonenumber);

public record VolunteerNullableDto(
    string? FullName,
    string? Email,
    string? Description,
    int? YOExperience,
    PhoneNumberDto? Phonenumber);