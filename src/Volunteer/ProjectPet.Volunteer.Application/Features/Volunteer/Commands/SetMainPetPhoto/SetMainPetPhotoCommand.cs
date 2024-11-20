namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.SetMainPetPhoto;

public record SetMainPetPhotoCommand(Guid VolunteerId, Guid Petid, string PhotoPath);
