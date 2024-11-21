namespace ProjectPet.Core.Providers;

public record FileDto(
    Stream Stream,
    string FilePath,
    string ContentType);