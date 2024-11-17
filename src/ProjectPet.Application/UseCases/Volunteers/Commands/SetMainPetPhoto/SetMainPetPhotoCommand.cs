namespace ProjectPet.Application.UseCases.Volunteers.Commands.SetMainPetPhoto;

public record SetMainPetPhotoCommand(Guid VolunteerId, Guid Petid, string PhotoPath);
