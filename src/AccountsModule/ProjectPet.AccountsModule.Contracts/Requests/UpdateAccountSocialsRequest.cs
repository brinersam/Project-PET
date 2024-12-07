using ProjectPet.AccountsModule.Contracts.Dto;

namespace ProjectPet.AccountsModule.Contracts.Requests;

public record class UpdateAccountSocialsRequest(List<SocialNetworkDto> SocialNetworks);
