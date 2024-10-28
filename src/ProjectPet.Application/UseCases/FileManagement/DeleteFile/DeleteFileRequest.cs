namespace ProjectPet.Application.UseCases.FileManagement.DeleteFile;

public record DeleteFileRequest(
    int DebugUserId,
    string[] FilesToDelete);
