namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeletePetPhotos;

public record DeletePetPhotosCommand(Guid volunteerId, Guid Petid, string[] PhotoPathsToDelete);
