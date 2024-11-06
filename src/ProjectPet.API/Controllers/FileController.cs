using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Contracts.FileManagement;
using ProjectPet.API.Extentions;
using ProjectPet.API.Processors;
using ProjectPet.Application.UseCases.FileManagement.DeleteFile;
using ProjectPet.Application.UseCases.FileManagement.Dto;
using ProjectPet.Application.UseCases.FileManagement.GetFile;
using ProjectPet.Application.UseCases.FileManagement.UploadFile;

namespace ProjectPet.API.Controllers;

public class FileController : CustomControllerBase
{
    [HttpPost("{debugUserId:int}")]
    public async Task<IActionResult> UploadFiles(
        [FromServices] UploadFileHandler service,
        [FromRoute] int debugUserId,
        [FromForm] UploadFileDto dto,
        CancellationToken cancellationToken = default)
    {
        await using (var processor = new FormFileProcessor())
        {
            List<FileDto> filesDto = processor.Process(dto.Files);

            var request = new UploadFileRequest(
                    debugUserId,
                    dto.Title,
                    filesDto);

            var result = await service.HandleAsync(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }

    [HttpGet("{debugUserId:int}")]
    public async Task<IActionResult> GetFilesInfo(
        [FromServices] GetFileInfoHandler service,
        [FromRoute] int debugUserId,
        CancellationToken cancellationToken = default)
    {
        var request = new GetFileRequest(debugUserId);
        var result = await service.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        if (result.Value.Count <= 0)
            return StatusCode(204);

        return Ok(result.Value);
    }

    [HttpDelete("{debugUserId:int}")]
    public async Task<IActionResult> DeleteFiles(
        [FromServices] DeleteFileHandler service,
        [FromBody] DeleteFileRequestDto dto,
        [FromRoute] int debugUserId,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteFileRequest(
            debugUserId,
            dto.FileNames);

        var result = await service.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}
