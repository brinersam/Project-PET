using ProjectPet.Application.UseCases.FileManagement.Dto;

namespace ProjectPet.Application.UseCases.FileManagement.UploadFile
{
    public record UploadFileRequest(
        int DebugId,
        string Title,
        List<FileDto> Files);
}