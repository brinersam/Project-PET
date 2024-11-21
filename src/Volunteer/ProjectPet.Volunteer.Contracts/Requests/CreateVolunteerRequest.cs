using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Contracts.Requests;

public record CreateVolunteerRequest(
    CreateVolunteerDto VolunteerDto,
    List<PaymentInfoDto>? PaymentInfoDtos,
    List<SocialNetworkDto>? SocialNetworkDtos);
