namespace ProjectPet.Core.Options;

public class OptionsMinIO
{
    public const string SECTION = "Minio";
    public string Endpoint { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public bool WithSSL { get; init; } = false;
    public int MaxConcurrentUpload { get; init; } = 5;
}
