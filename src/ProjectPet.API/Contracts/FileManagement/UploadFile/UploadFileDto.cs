namespace ProjectPet.API.Contracts.FileManagement.UploadFile;

public record UploadFileDto(
    string Title,
    IFormFileCollection Files);


