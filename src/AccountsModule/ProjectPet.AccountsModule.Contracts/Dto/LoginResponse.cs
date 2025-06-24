namespace ProjectPet.AccountsModule.Contracts.Dto;
public record LoginResponse(Guid RefreshToken, string AccessToken);
