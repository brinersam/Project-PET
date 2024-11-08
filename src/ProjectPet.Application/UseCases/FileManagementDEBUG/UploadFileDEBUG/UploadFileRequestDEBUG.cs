using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.FileManagementDEBUG.UploadFileDebug;

public record UploadFileRequestDEBUG(
    int DebugId,
    string Title,
    List<FileDto> Files);