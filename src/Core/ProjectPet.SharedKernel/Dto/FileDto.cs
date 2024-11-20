namespace ProjectPet.Core.Dto;

public record FileDto(
    Stream Stream,
    string FilePath,
    string ContentType);