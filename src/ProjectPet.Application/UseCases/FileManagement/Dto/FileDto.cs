namespace ProjectPet.Application.UseCases.FileManagement
{
    public record FileDto(
        Stream Stream,
        string FilePath,
        string ContentType);
}