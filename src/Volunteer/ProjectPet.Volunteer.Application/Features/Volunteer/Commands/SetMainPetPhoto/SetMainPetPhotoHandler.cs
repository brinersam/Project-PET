using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.SetMainPetPhoto;

public class SetMainPetPhotoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;

    public SetMainPetPhotoHandler(IVolunteerRepository volunteerRepository)
    {
        _volunteerRepository = volunteerRepository;
    }
    public async Task<UnitResult<Error>> HandleAsync(SetMainPetPhotoCommand cmd, CancellationToken cancellationToken)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(cmd.VolunteerId, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        var petRes = volunteer.GetPetById(cmd.Petid);
        if (petRes.IsFailure)
            return petRes.Error;
        var pet = petRes.Value;

        var setPhotoRes = pet.SetMainPhoto(cmd.PhotoPath);
        if (setPhotoRes.IsFailure)
            return setPhotoRes.Error;

        await _volunteerRepository.Save(volunteer, cancellationToken);

        return Result.Success<Error>();
    }
}
