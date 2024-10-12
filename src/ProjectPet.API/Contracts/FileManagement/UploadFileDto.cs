namespace ProjectPet.Application.UseCases.FileManagement
{
    public record UploadFileDto(
        string Title,
        IFormFile[] Files);
}

    