namespace ProjectPet.AccountsModule.Infrastructure.Options;
public class AdminCredsOptions
{
    public const string SECTION = "ADMIN";
    public string USERNAME { get; init; } = String.Empty;
    public string PASSWORD { get; init; } = String.Empty;
    public string EMAIL { get; init; } = String.Empty;
}
