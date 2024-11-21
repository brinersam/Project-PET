using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Contracts.Requests;

public record class UpdateVolunteerSocialsRequest(List<SocialNetworkDto> SocialNetworks);
