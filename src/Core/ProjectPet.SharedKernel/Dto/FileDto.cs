namespace ProjectPet.SharedKernel.Dto;

public record FileDto(
    Stream Stream,
    string FilePath,
    string ContentType);