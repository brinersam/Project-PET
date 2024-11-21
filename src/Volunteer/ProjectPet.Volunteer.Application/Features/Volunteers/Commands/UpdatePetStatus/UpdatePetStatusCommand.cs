using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid Petid, PetStatusDto Status);
