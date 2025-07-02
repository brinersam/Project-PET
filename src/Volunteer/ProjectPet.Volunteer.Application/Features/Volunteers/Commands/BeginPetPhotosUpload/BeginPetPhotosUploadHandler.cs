using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.FileService.Contracts;
using ProjectPet.FileService.Contracts.Dtos;
using ProjectPet.FileService.Contracts.Features.MultipartPutChunkUrlUpload;
using ProjectPet.FileService.Contracts.Features.MultipartStartUpload;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UploadPetPhoto;

public class BeginPetPhotosUploadHandler
{
    private static HashSet<string> allowedContentTypes = ["image/jpg", "image/jpeg", "image/png"];

    private readonly ILogger<BeginPetPhotosUploadHandler> _logger;
    private readonly IFileService _fileService;
    private readonly IVolunteerRepository _volunteerRepository;

    public BeginPetPhotosUploadHandler(
        ILogger<BeginPetPhotosUploadHandler> logger,
        IFileService fileService,
        IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _fileService = fileService;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<List<BeginPetPhotoUploadDto>, Error>> HandleAsync(
        BeginPetPhotosUploadCommand request,
        CancellationToken cancellationToken = default)
    {
        var volunteerIdRes = await _volunteerRepository.GetByIdAsync(request.VolunteerId, cancellationToken);
        if (volunteerIdRes.IsFailure)
            return volunteerIdRes.Error;
        var volunteer = volunteerIdRes.Value;

        var petRes = volunteer.GetPetById(request.PetId);
        if (petRes.IsFailure)
            return petRes.Error;

        var uploadRequests = GenerateUploadStartRequests(request);
        var uploadDatasRes = await StartUploadsAndReturnUrlsAsync(uploadRequests, cancellationToken);

        _logger.LogInformation(
            "Successfully started photo (amount of {O1}) uploads for a pet (id : {O2}) of a user (id : {O3})",
            uploadDatasRes.Count(x => x.Error is null),
            request.PetId,
            request.VolunteerId);

        return uploadDatasRes.ToList();
    }

    private List<(MultipartStartUploadRequest request, string fileName)> GenerateUploadStartRequests(BeginPetPhotosUploadCommand request)
    {
        List<(MultipartStartUploadRequest request, string fileName)> uploadRequests = [];

        foreach (var file in request.FileUploadDtos)
        {
            if (allowedContentTypes.Contains(file.ContentType) == false)
            {
                _logger.LogInformation($"Skipping file with disallowed ContentType: ({file.ContentType})");
                continue;
            }

            uploadRequests.Add(
                (
                    new MultipartStartUploadRequest(
                        file.FileName, file.ContentType, file.SizeBytes, file.BucketName),
                    file.FileName
                )
            );
        }
        return uploadRequests;
    }

    private async Task<BeginPetPhotoUploadDto[]> StartUploadsAndReturnUrlsAsync(
        List<(MultipartStartUploadRequest request, string fileName)> uploadStartRequests,
        CancellationToken ct = default)
    {
        BeginPetPhotoUploadDto[] uploadUrlsTaskResults =
            await Task.WhenAll(
                uploadStartRequests.Select(async req =>
                {
                    var uploadStartResult = await _fileService.MultipartStartUpload(req.request, ct);
                    if (uploadStartResult.IsFailure)
                        return UploadFailedDto(uploadStartResult.Error);
                    var uploadResponse = uploadStartResult.Value;

                    var generateUrlsResult = await GenerateMultipartUploadUrlsAsync(uploadResponse, req.fileName, ct);
                    if (generateUrlsResult.IsFailure)
                        return UploadFailedDto(generateUrlsResult.Error);

                    return generateUrlsResult.Value;
                })
            );


        return uploadUrlsTaskResults;
    }

    private async Task<Result<BeginPetPhotoUploadDto, Error>> GenerateMultipartUploadUrlsAsync(MultipartStartUploadResponse uploadResponse, string fileName, CancellationToken ct)
    {
        List<Task<Result<MultipartPutChunkUrlResponse, Error>>> urlTasks = [];
        for (int xthChunk = 1; xthChunk <= uploadResponse.TotalChunks; xthChunk++)
        {
            urlTasks.Add(_fileService.MultipartPutChunkUrlUploadAsync(
                    new MultipartPutChunkUrlRequest(
                        new FileLocationDto(
                            uploadResponse.location.FileId,
                            uploadResponse.location.BucketName
                        ),
                        uploadResponse.UploadId,
                        xthChunk
                    ),
                    ct
                )
            );
        }

        var urlTasksResult = await Task.WhenAll(urlTasks);
        if (urlTasksResult.Any(x => x.IsFailure))
            return Error.Failure("create.urls.failure", urlTasksResult.First(x => x.IsFailure).Error.Message);

        return new BeginPetPhotoUploadDto(
            null!,
            fileName,
            uploadResponse.location.FileId,
            uploadResponse.UploadId,
            urlTasksResult.Select(x => x.Value.Url).ToArray());
    }

    private BeginPetPhotoUploadDto UploadFailedDto(Error error)
    {
        return new BeginPetPhotoUploadDto(
                    error,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    []
                );
    }
}
