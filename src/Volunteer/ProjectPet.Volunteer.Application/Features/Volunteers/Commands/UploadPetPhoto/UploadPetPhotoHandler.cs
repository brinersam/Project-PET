using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Database;
using ProjectPet.Core.Files;
using ProjectPet.Core.MessageQueues;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UploadPetPhoto;

public class UploadPetPhotoHandler
{
    private static HashSet<string> allowedFileExtensions = [".png", ".jpg"];
    private readonly string BUCKETNAME = Constants.PET_PHOTOS_BUCKETNAME;

    private readonly ILogger<UploadPetPhotoHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IMessageQueue<IEnumerable<FileDataDto>> _messageQueue;

    public UploadPetPhotoHandler(
        ILogger<UploadPetPhotoHandler> logger,
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        IVolunteerRepository volunteerRepository,
        IMessageQueue<IEnumerable<FileDataDto>> messageQueue)
    {
        _logger = logger;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _volunteerRepository = volunteerRepository;
        _messageQueue = messageQueue;
    }

    public async Task<Result<List<string>, Error>> HandleAsync(
        UploadPetPhotoCommand request,
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

                fileList.Add(new FileDataDto(file.Stream, fileName, volunteer.Id, BUCKETNAME));
            }

            var fileuploadRes = await _fileProvider.UploadFilesAsync(
                fileList,
                BUCKETNAME,
                volunteer.Id,
                cancellationToken);

            if (fileuploadRes.IsSuccess == false)
            {
                await _messageQueue.WriteAsync(fileList, cancellationToken);
                return fileuploadRes.Error;
            }


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
