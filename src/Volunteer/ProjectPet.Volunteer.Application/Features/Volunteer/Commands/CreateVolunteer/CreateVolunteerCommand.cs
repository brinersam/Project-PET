using ProjectPet.SharedKernel.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.CreateVolunteer;

public record CreateVolunteerCommand(
    CreateVolunteerDto VolunteerDto,
    List<PaymentInfoDto>? PaymentInfoDtos,
    List<SocialNetworkDto>? SocialNetworkDtos);
