using ProjectPet.SharedKernel.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdateVolunteerInfo;

public record UpdateVolunteerInfoCommand(
    Guid Id,
    CreateVolunteerNullableDto Dto);
