using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Contracts.FileManagement;
using ProjectPet.API.Processors;
using ProjectPet.Application.UseCases.FileManagement;

namespace ProjectPet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        [HttpPost("{DebugUserId:int}")]
        public async Task<IActionResult> UploadFile(
            [FromServices] UploadFileHandler service,
            [FromRoute] int DebugUserId, // test controller
            [FromForm] UploadFileDto dto,
            [FromServices] IValidator<UploadFileDto> validator,
            CancellationToken cancellationToken = default)
        {
            var validatorRes = await validator.ValidateAsync(dto);
            if (validatorRes.IsValid == false)
                return BadRequest(validatorRes.Errors); // todo refactor to use envelope

            await using (var processor = new FormFileProcessor())
            {
                List<FileDto> filesDto = processor.Process(dto.Files);

                var request = new UploadFileRequest(DebugUserId,
                    dto.Title,
                    filesDto);

                var result = await service.HandleAsync(request, cancellationToken);

                if (result.IsFailure)
                    return BadRequest(result.Error.Message);
                // todo refactor to handle different error codes

                return Ok(result.Value);
            }
        }

        //[HttpDelete("{DebugUserId:int}")]
        //public async Task<IActionResult> DeleteFile(
        //    [FromServices] DeleteFileHandler service,
        //    [FromRoute] int DebugUserId,
        //    [FromBody] DeleteFileDto dto,
        //    [FromServices] IValidator<UploadFileDto> validator,
        //    CancellationToken cancellationToken = default)
        //{
        //    await using var stream = file.OpenReadStream();

        //}

        [HttpGet("{debugUserId:int}")]
        public async Task<IActionResult> GetFileForId(
            [FromServices] GetFileHandler service,
            [FromRoute] int debugUserId,
            CancellationToken cancellationToken = default)
        {
            var request = new GetFileRequest(debugUserId);
            var result = await service.Handle(request, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            if (result.Value.Count <= 0)
                return StatusCode(204);

            return Ok(result.Value);
        }
    }

}
