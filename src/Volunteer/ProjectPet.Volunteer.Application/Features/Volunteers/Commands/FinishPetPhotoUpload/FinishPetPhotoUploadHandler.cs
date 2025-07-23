using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.FileService.Contracts;
using ProjectPet.FileService.Contracts.Features.MultipartFinishUpload;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.FinishPetPhotoUpload;
public class FinishPetPhotoUploadHandler
{
    private readonly ILogger<FinishPetPhotoUploadHandler> _logger;
    private readonly IFileService _fileService;
    private readonly IVolunteerRepository _volunteerRepository;

    public FinishPetPhotoUploadHandler(
        ILogger<FinishPetPhotoUploadHandler> logger,
        IFileService fileService,
        IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _fileService = fileService;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<List<ResultPetPhotoUploadDto>, Error>> HandleAsync(
        FinishPetPhotoUploadCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerIdRes = await _volunteerRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
        if (volunteerIdRes.IsFailure)
            return volunteerIdRes.Error;
        var volunteer = volunteerIdRes.Value;

        var petRes = volunteer.GetPetById(command.PetId);
        if (petRes.IsFailure)
            return petRes.Error;
        var pet = petRes.Value;

        var finishFileUploadTasks = command.Files.Select(async finishCmd =>
            {
                var request = new MultipartFinishUploadRequest(finishCmd.uploadData.Location, finishCmd.uploadData.UploadId, finishCmd.uploadData.Etags.ToList());
                var uploadResult = await _fileService.MultipartFinishUploadAsync(request, cancellationToken);
                var response = new PetPhotoUploadData(finishCmd.uploadData.Location, finishCmd.FileName, finishCmd.ContentType);

                if (uploadResult.IsFailure)
                    return new ResultPetPhotoUploadDto(uploadResult.Error, response);

                return new ResultPetPhotoUploadDto(null!, new PetPhotoUploadData(uploadResult.Value.location, response.FileName, response.ContentType));

            });

        ResultPetPhotoUploadDto[] finishFileUploadTasksResults = await Task.WhenAll(finishFileUploadTasks);

        pet.AddPhotos(finishFileUploadTasksResults
                            .Where(x => x.Error is null)
                            .Select(x => PetPhoto.Create(
                                    x.uploadData.Location.FileId,
                                    x.uploadData.Location.BucketName,
                                    x.uploadData.FileName,
                                    x.uploadData.ContentType).Value
                            )
        );

        _logger.LogInformation("Attempted to uploaded {p1uploadAmount} photos for a pet (id: {p2petid}) of a volunteer (id: {p3VolunteerId}) : \n{p4files}",
            finishFileUploadTasksResults.Length,
            command.PetId,
            command.VolunteerId,
            String.Join(";\n ", finishFileUploadTasksResults.Select(x =>
            {
                var result = x.uploadData;
                var location = x.uploadData.Location;
                var successPrefis = x.Error is null ? "SUCCESS!" : "ERRORED!";
                return $"{successPrefis}: Filename: {result.FileName}, ContentType: {result.ContentType}, Id: {location.FileId}, Bucket: {location.BucketName}";
            })));

        var saveRes = await _volunteerRepository.Save(volunteer, cancellationToken);
        if (saveRes.IsFailure)
        {
            return saveRes.Error;
            // todo eventual consistency to file service -> delete uploads
        }

        return finishFileUploadTasksResults.ToList();
    }
}