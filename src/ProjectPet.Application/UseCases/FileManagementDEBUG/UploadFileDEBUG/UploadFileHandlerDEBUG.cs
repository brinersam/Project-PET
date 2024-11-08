//using CSharpFunctionalExtensions;
//using Microsoft.Extensions.Logging;
//using ProjectPet.Application.Database;
//using ProjectPet.Application.Providers;
//using ProjectPet.Application.UseCases.Volunteers.UploadPetPhoto;
//using ProjectPet.Domain.Shared;

//namespace ProjectPet.Application.UseCases.FileManagementDEBUG.UploadFileDebug;

//public class UploadFileHandlerDEBUG
//{
//    private const string BUCKETNAME = "photos";

//    private readonly ILogger<UploadFileHandlerDEBUG> _logger;
//    private readonly IFileProvider _fileProvider;
//    private readonly IUnitOfWork _unitOfWork;

//    public UploadFileHandlerDEBUG(
//        ILogger<UploadFileHandlerDEBUG> logger,
//        IFileProvider fileProvider,
//        IUnitOfWork unitOfWork)
//    {
//        _logger = logger;
//        _fileProvider = fileProvider;
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<Result<List<string>, Error>> HandleAsync(
//        UploadPetPhotoRequest request,
//        CancellationToken cancellationToken = default)
//    {
//        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

//        try
//        {
//            //var volunteerIdRes = await _repository.GetByIdAsync(request.Id);
//            //if (volunteerIdRes.IsFailure)
//            //    return volunteerIdRes.Errors;
//            var volunteerId = request.DebugId; // while we use debugid we always succeed



//            var fileList = request.Files.Select(file =>
//                new FileDataDto(
//                    file.Stream,
//                    $"{request.Title}_{Guid.NewGuid()}{Path.GetExtension(file.FilePath)}")
//                )
//                .ToList();

//            await _unitOfWork.SaveChangesAsync(cancellationToken);

//            var fileuploadRes = await _fileProvider.UploadFilesAsync(
//                fileList,
//                BUCKETNAME,
//                volunteerId,
//                cancellationToken);

//            if (fileuploadRes.IsSuccess == false)
//                return fileuploadRes.Error;

//            transaction.Commit();

//            _logger.LogInformation("Successfully uploaded files {files} to a bucket {bucket} of user with id {id}",
//                    string.Join(" | ", fileuploadRes.Value), BUCKETNAME, request.DebugId);

//            return fileuploadRes.Value;
//        }
//        catch (Exception ex)
//        {
//            transaction.Rollback();
//            return Error.Failure("failure", ex.Message);
//        }
//    }
//}
