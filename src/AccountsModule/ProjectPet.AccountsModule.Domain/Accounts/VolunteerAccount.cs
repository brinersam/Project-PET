namespace ProjectPet.AccountsModule.Domain.Accounts;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record VolunteerAccount
{
    public Guid UserId { get; }
    public int Experience { get; }
    public string[] ContactInformation { get; } //todo vo
    public string[] Certifications { get; } //todo vo

}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.