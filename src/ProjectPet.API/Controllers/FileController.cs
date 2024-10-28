using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Contracts.FileManagement;
using ProjectPet.API.Extentions;
using ProjectPet.API.Processors;
using ProjectPet.API.Response;
using ProjectPet.Application.UseCases.FileManagement;

namespace ProjectPet.API.Controllers;

public class FileController : CustomControllerBase
{
    [HttpPost("{debugUserId:int}")]
    public async Task<IActionResult> UploadFiles(
        [FromServices] UploadFileHandler service,
        [FromServices] IValidator<UploadFileDto> validator,
        [FromRoute] int debugUserId,
        [FromForm] UploadFileDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = validator.Validate(dto);
        if (validationResult.IsValid == false)
            return Envelope.ToResponse(validationResult.Errors);

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
