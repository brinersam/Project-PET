namespace ProjectPet.API.Contracts.FileManagement
{
    public record UploadFileDto(
        string Title,
        IFormFile[] Files);
}

