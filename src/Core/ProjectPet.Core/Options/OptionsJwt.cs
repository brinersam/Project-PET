namespace ProjectPet.Core.Options;
public class OptionsJwt
{
    public const string SECTION = nameof(OptionsJwt);
    public string Issuer { get; init; } 
    public string Audience { get; init; } 
    public string Key { get; init; } 
    public int ExpiresMin { get; init; } 
}
