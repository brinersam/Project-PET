namespace ProjectPet.AccountsModule.Domain.Accounts;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record MemberAccount()
{
    public const string ROLENAME = "Member";
    public List<Guid> FavoritePetIds { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.