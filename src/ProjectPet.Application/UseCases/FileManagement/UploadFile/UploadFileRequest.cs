namespace ProjectPet.Application.UseCases.FileManagement;

public record UploadFileRequest(
    int DebugId,
    string Title,
    List<FileDto> Files);