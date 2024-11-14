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

public class VolunteerReadDto
{
    public Guid Id { get; init; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Description { get; init; }
    public int YOExperience { get; init; }
    public string Phonenumber { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
};