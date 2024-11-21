using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid Petid, PetStatus Status);
