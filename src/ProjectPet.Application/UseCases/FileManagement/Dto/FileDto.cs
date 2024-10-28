namespace ProjectPet.Application.UseCases.FileManagement.Dto;

public record FileDto(
    Stream Stream,
    string FilePath,
    string ContentType);