using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid Petid, PetStatus Status);
