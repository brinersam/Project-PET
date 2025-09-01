namespace DEVShared;

public class OptionsTokens
{
    public const string SECTION = "OptionsTokens";

    public bool GenerateTokens { get; init; } = false;

    public string Issuer { get; init; } = string.Empty;


    public string Audience { get; init; } = string.Empty;


    public string Key { get; init; } = string.Empty;


    public int AccessExpiresMin { get; init; }

    public int RefreshExpiresMin { get; init; }
}