namespace ProjectPet.Application.Dto;

public record FileDto(
    Stream Stream,
    string FilePath,
    string ContentType);