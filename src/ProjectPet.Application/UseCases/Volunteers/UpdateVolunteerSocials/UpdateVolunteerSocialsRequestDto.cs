using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerSocials;

public record class UpdateVolunteerSocialsRequestDto(
    List<SocialNetworkDto> SocialNetworks)
{ }
