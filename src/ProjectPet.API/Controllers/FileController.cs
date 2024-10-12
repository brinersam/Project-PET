using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Processors;
using ProjectPet.Application.UseCases.FileManagement;
using ProjectPet.Application.UseCases.FileManagement.Dto;
using ProjectPet.Application.UseCases.FileManagement.UploadFile;

namespace ProjectPet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        [HttpPost("{DebugId:int}")]
        public async Task<IActionResult> UploadFile(
            [FromServices] UploadFileHandler service,
            [FromRoute] int DebugId, // this is a test controller, we do not care for an id outside of debug purposes
            [FromForm] UploadFileDto dto,
            CancellationToken cancellationToken = default)
        {
            await using (var processor = new FormFileProcessor())
            {
                List<FileDto> filesDto = processor.Process(dto.Files);

                var request = new UploadFileRequest(DebugId,
                    dto.Title,
                    filesDto);

                var result = await service.HandleAsync(request, cancellationToken);

                if (result.IsFailure)
                    return BadRequest(result.Error.Message);
                // todo refactor to handle different error codes

                return Ok(result.Value);
            }
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteFile(
        //    IFormFile file,
        //    CancellationToken cancellationToken = default)
        //{
        //    await using var stream = file.OpenReadStream();

        //}

        //[HttpGet]
        //public async Task<IActionResult> GetFileForId(
        //    IFormFile file,
        //    CancellationToken cancellationToken = default)
        //{
        //    await using var stream = file.OpenReadStream();

        //}
    }
}
