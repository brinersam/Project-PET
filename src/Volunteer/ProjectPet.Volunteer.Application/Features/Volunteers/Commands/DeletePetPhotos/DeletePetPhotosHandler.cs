using CSharpFunctionalExtensions;
using ProjectPet.FileService.Contracts;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Responses;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeletePetPhotos;

public class DeletePetPhotosHandler
{
    private readonly IFileService _fileService;
    private readonly IVolunteerRepository _volunteerRepository;

    public DeletePetPhotosHandler(
        IFileService fileService,
        IVolunteerRepository volunteerRepository)
    {
        _fileService = fileService;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<List<DeletePetPhotosResponse>, Error>> HandleAsync(DeletePetPhotosCommand cmd, CancellationToken cancellationToken)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(cmd.volunteerId, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        var pet = volunteer.OwnedPets.FirstOrDefault(x => x.Id == cmd.Petid);
        if (pet is null)
            return Errors.General.NotFound(typeof(Pet), cmd.Petid);

        var idsToDelete = cmd.PhotoFileIdsToDelete.ToHashSet();

        var deleteFilesTasksResults = await Task.WhenAll(
                pet.Photos.Where(x => idsToDelete.Contains(x.FileId)).Select(async x =>
                    {
                        var result = await _fileService.DeleteFileAsync(new(x.FileId, x.BucketName));
                        return new DeletePetPhotosResponse(
                            result.IsFailure ? result.Error : null,
                            x.FileId
                        );
                    }
                )
        );

        volunteer.DeletePetPhotos(
            cmd.Petid,
            deleteFilesTasksResults
                .Where(x => x.Error is null)
                .Select(x => x.FileId)
        );

        await _volunteerRepository.Save(volunteer, cancellationToken);

        return deleteFilesTasksResults.ToList();
    }
}
