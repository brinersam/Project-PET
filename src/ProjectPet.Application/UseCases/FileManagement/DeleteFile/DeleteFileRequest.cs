namespace ProjectPet.Application.UseCases.FileManagement
{
    public record DeleteFileRequest(
        int DebugUserId,
        string[] FilesToDelete);
}
