namespace ProjectPet.AccountsModule.Infrastructure.Options;
public class AdminCredsOptions
{
    public const string SECTION = "ADMIN";
    public string USERNAME { get; init; } = "DEFAULTADMINUSERNAME";
    public string PASSWORD { get; init; } = "Aa1#1234567890-123456";
    public string EMAIL { get; init; } = "DEFAULTADMINUSERNAME@mail.com";
}
