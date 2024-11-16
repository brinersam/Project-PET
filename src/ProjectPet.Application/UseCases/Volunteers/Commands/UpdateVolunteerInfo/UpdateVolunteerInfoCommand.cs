using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerInfo;

public record UpdateVolunteerInfoCommand(
    Guid Id,
    VolunteerNullableDto Dto);
