namespace ProjectPet.Application.UseCases.Volunteers;

public record class UpdateVolunteerSocialsRequestDto(
    List<SocialNetworkDto> SocialNetworks)
{ }
