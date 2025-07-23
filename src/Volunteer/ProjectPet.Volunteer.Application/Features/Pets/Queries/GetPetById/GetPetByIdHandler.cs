using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPet.FileService.Contracts;
using ProjectPet.FileService.Contracts.Dtos;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Responses;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetById;

public class GetPetByIdHandler
{
    private readonly IFileService _fileService;
    private readonly ILogger<GetPetByIdHandler> _logger;
    private readonly IReadDbContext _readDbContext;
    private readonly IVolunteerRepository _rep;

    public GetPetByIdHandler(
        IFileService fileService,
        ILogger<GetPetByIdHandler> loggger,
        IReadDbContext readDbContext)
    {
        _fileService = fileService;
        _logger = loggger;
        _readDbContext = readDbContext;
    }

    public async Task<Result<PetResponse, Error>> HandleAsync(
        GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        var pet = await _readDbContext.Pets.FirstOrDefaultAsync(
            x => x.Id == query.Petid,
            cancellationToken);

        if (pet is null)
            return Errors.General.NotFound(typeof(Pet));

        if (pet.Photos.Count <= 0)
            return PetResponse.FromRequest(pet, []);

        var photoResponseList = await _fileService.PresignedUrlsDownloadAsync(
                new(
                    pet.Photos
                        .Select(x => new FileLocationDto(x.FileId, x.BucketName))
                        .ToList()
                ),
                cancellationToken
            );

        if (photoResponseList.IsFailure)
        {
            _logger.LogError("Failed to retrieve photos for pet (id: {p1}) with error {p2}", query.Petid, photoResponseList.Error.Message);
            return PetResponse.FromRequest(pet, []);
        }

        var urls = photoResponseList.Value.Urls.ToDictionary(x => x.FileId, x => x.Url);
        var photoResponses = pet.Photos.Select(x => new PetPhotoResponse(x.FileName, x.FileId, urls[x.FileId]));

        return PetResponse.FromRequest(pet, photoResponses);
    }
}
