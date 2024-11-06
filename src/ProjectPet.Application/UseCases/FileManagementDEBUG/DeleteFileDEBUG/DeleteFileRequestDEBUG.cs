namespace ProjectPet.Application.UseCases.FileManagementDEBUG.DeleteFileDEBUG;

public record DeleteFileRequestDEBUG(
    int DebugUserId,
    string[] FilesToDelete);
