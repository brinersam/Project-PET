using CSharpFunctionalExtensions;
using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.DeletePetPhotos;

public class DeletePetPhotosHandler
{
    private readonly string BUCKETNAME = Constants.PET_PHOTOS_BUCKETNAME;
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;

    public DeletePetPhotosHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
    }
    public async Task<UnitResult<Error>> HandleAsync(DeletePetPhotosCommand cmd, CancellationToken cancellationToken)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(cmd.volunteerId, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        volunteer.DeletePhotos(cmd.Petid, cmd.PhotoPathsToDelete);

        var deleteFilesResult = await _fileProvider.DeleteFilesAsync(
            BUCKETNAME,
            cmd.volunteerId,
            cmd.PhotoPathsToDelete,
            cancellationToken);

        if (deleteFilesResult.IsFailure)
            return deleteFilesResult.Error;

        await _volunteerRepository.Save(volunteer, cancellationToken);

        return Result.Success<Error>();
    }
}
