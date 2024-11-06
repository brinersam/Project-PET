using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Database;
using ProjectPet.Application.Providers;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.UploadPetPhoto;

public class UploadPetPhotoHandler
{
    private static HashSet<string> allowedFileExtensions = [".png", ".jpg"];
    private const string BUCKETNAME = "pet-photos";

    private readonly ILogger<UploadPetPhotoHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRepository _volunteerRepository;

    public UploadPetPhotoHandler(
        ILogger<UploadPetPhotoHandler> logger,
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<List<string>, Error>> HandleAsync(
        UploadPetPhotoRequest request,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var volunteerIdRes = await _volunteerRepository.GetByIdAsync(request.VolunteerId, cancellationToken);
            if (volunteerIdRes.IsFailure)
                return volunteerIdRes.Error;
            var volunteer = volunteerIdRes.Value;

            var petRes = volunteer.GetPetById(request.PetId);
            if (petRes.IsFailure)
                return petRes.Error;
            var pet = petRes.Value;

            List<FileDataDto> fileList = [];

            foreach (var file in request.Files)
            {
                var fileExtension = Path.GetExtension(file.FilePath);

                if (allowedFileExtensions.Contains(fileExtension) == false)
                {
                    _logger.LogInformation($"Skipping file with disallowed extension: ({fileExtension})");
                    continue;
                }

                var fileName = _fileProvider.FileNamer(
                    request.Title,
                    $"Pet:{pet.Id}",
                    Guid.NewGuid().ToString(),
                    fileExtension);

                fileList.Add(new FileDataDto(file.Stream, fileName));
            }

            var fileuploadRes = await _fileProvider.UploadFilesAsync(
                fileList,
                BUCKETNAME,
                volunteer.Id,
                cancellationToken);

            if (fileuploadRes.IsSuccess == false)
                return fileuploadRes.Error;

            pet.AddPhotos(fileuploadRes.Value.Select(x => PetPhoto.Create(x).Value));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();

            _logger.LogInformation("Successfully uploaded photos: \n{files} for a pet with id : {petid} to a bucket {bucket} of user with id {id}",
                    string.Join(" |\n ", fileuploadRes.Value), request.PetId, BUCKETNAME, request.VolunteerId);

            return fileuploadRes.Value;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Error.Failure("failure", ex.Message);
        }
    }
}
