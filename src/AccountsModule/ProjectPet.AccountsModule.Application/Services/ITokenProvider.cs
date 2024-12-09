using ProjectPet.AccountsModule.Domain;

namespace ProjectPet.AccountsModule.Application.Services;
public interface ITokenProvider
{
    string GenerateJwtAccessToken(User user);
}