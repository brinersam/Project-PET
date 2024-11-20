namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.DeletePetPhotos;

public record DeletePetPhotosCommand(Guid volunteerId, Guid Petid, string[] PhotoPathsToDelete);
