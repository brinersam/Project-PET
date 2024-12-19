namespace ProjectPet.Core.Options;
public class OptionsTokens
{
    public const string SECTION = nameof(OptionsTokens);
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string Key { get; init; }
    public int AccessExpiresMin { get; init; }
    public int RefreshExpiresMin { get; init; }
}
