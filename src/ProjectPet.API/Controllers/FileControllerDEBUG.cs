//using Microsoft.AspNetCore.Mvc;
//using ProjectPet.API.Contracts.FileManagement;
//using ProjectPet.API.Extentions;
//using ProjectPet.API.Processors;
//using ProjectPet.Application.Dto;
//using ProjectPet.Application.UseCases.FileManagementDEBUG.DeleteFileDEBUG;
//using ProjectPet.Application.UseCases.FileManagementDEBUG.GetFileDebug;
//using ProjectPet.Application.UseCases.FileManagementDEBUG.UploadFileDebug;

//namespace ProjectPet.API.Controllers;

//public class FileControllerDEBUG : CustomControllerBase
//{
//    [HttpPost("{debugUserId:int}")]
//    public async Task<IActionResult> UploadFiles(
//        [FromServices] UploadFileHandlerDEBUG service,
//        [FromRoute] int debugUserId,
//        [FromForm] UploadFileDto dto,
//        CancellationToken cancellationToken = default)
//    {
//        await using (var processor = new FormFileProcessor())
//        {
//            List<FileDto> filesDto = processor.Process(dto.Files);

//            var request = new UploadPetPhotoRequest(
//                    debugUserId,
//                    dto.Title,
//                    filesDto);

//            var result = await service.HandleAsync(request, cancellationToken);

//            if (result.IsFailure)
//                return result.Error.ToResponse();

//            return Ok(result.Value);
//        }
//    }

//    [HttpGet("{debugUserId:int}")]
//    public async Task<IActionResult> GetFilesInfo(
//        [FromServices] GetFileInfoHandlerDEBUG service,
//        [FromRoute] int debugUserId,
//        CancellationToken cancellationToken = default)
//    {
//        var request = new GetFileRequestDEBUG(debugUserId);
//        var result = await service.Handle(request, cancellationToken);
//        if (result.IsFailure)
//            return result.Error.ToResponse();

//        if (result.Value.Count <= 0)
//            return StatusCode(204);

//        return Ok(result.Value);
//    }

//    [HttpDelete("{debugUserId:int}")]
//    public async Task<IActionResult> DeleteFiles(
//        [FromServices] DeleteFileHandlerDEBUG service,
//        [FromBody] DeleteFileRequestDto dto,
//        [FromRoute] int debugUserId,
//        CancellationToken cancellationToken = default)
//    {
//        var request = new DeleteFileRequestDEBUG(
//            debugUserId,
//            dto.FileNames);

//        var result = await service.Handle(request, cancellationToken);
//        if (result.IsFailure)
//            return result.Error.ToResponse();

//        return Ok();
//    }
//}
