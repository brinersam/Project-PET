namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.SetMainPetPhoto;

public record SetMainPetPhotoCommand(Guid VolunteerId, Guid Petid, string PhotoPath);
