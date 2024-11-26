using ProjectPet.AccountsModule.Application.Models;

namespace ProjectPet.AccountsModule.Application.Services;
public interface ITokenProvider
{
    string GenerateJwtAccessToken(User user);
}