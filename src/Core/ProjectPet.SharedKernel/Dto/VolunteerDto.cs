namespace ProjectPet.Core.Dto;

public record CreateVolunteerDto(
    string FullName,
    string Email,
    string Description,
    int YOExperience,
    PhonenumberDto Phonenumber);

public record CreateVolunteerNullableDto(
    string? FullName,
    string? Email,
    string? Description,
    int? YOExperience,
    PhonenumberDto? Phonenumber);

public class VolunteerDto
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