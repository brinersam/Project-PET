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
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Description { get; init; }
    public int YOExperience { get; init; }
    public string Phonenumber { get; init; }
    public List<string> OwnedPets { get; init; }
    public List<string> PaymentMethods { get; init; }
    public List<string> SocialNetworks { get; init; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public VolunteerReadDto() { } // efcore
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public VolunteerReadDto(
        Guid Id,
        string FullName,
        string Email,
        string Description,
        int YOExperience,
        string Phonenumber,
        List<string> OwnedPets,
        List<string> PaymentMethods,
        List<string> SocialNetworks)
    {
        this.Id = Id;
        this.FullName = FullName;
        this.Email = Email;
        this.Description = Description;
        this.YOExperience = YOExperience;
        this.Phonenumber = Phonenumber;
        this.OwnedPets = OwnedPets;
        this.PaymentMethods = PaymentMethods;
        this.SocialNetworks = SocialNetworks;
    }
};