namespace ProjectPet.AccountsModule.Domain.Accounts;

public record AdminAccount(string FullName)
{
    public const string ROLENAME = "Admin";
};
