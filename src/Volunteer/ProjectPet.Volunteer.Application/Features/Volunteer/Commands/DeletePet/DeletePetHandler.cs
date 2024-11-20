using CSharpFunctionalExtensions;
using ProjectPet.Application.Database;
using ProjectPet.Application.Providers;
using ProjectPet.Domain.Shared;
using ProjectPet.VolunteerModule.Application.Interfaces;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.DeletePet;

public class DeletePetHandler
{
    private readonly string BUCKETNAME = Constants.PET_PHOTOS_BUCKETNAME;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePetHandler(
        IVolunteerRepository volunteerRepository,
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
    }
    public async Task<UnitResult<Error>> HandleAsync(DeletePetCommand cmd, CancellationToken cancellationToken)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(cmd.VolunteerId, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        if (cmd.SoftDelete)
        {
            var deletionResult = volunteer.SoftDeletePet(cmd.PetId);
            if (deletionResult.IsFailure)
                return deletionResult.Error;

            await _volunteerRepository.Save(volunteer, cancellationToken);

            return Result.Success<Error>();
        }
        else
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var petRes = volunteer.GetPetById(cmd.PetId);
            if (petRes.IsFailure)
                return petRes.Error;

            var photoPaths = petRes.Value.Photos.Data.Select(x => x.StoragePath).ToArray();

            var deletePetRes = volunteer.DeletePet(cmd.PetId);
            if (deletePetRes.IsFailure)
                return deletePetRes.Error;

            petRes.Value.DeletePhotos(photoPaths);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var deleteFilesResult = await _fileProvider.DeleteFilesAsync(
                BUCKETNAME,
                cmd.VolunteerId,
                photoPaths,
                cancellationToken);

            if (deleteFilesResult.IsFailure)
            {
                transaction.Rollback();
                return deleteFilesResult.Error;
            }

            transaction.Commit();
        }

        return Result.Success<Error>();
    }
}