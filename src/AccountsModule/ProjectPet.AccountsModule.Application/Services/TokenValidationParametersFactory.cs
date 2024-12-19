using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectPet.Core.Options;
using System.Text;

namespace ProjectPet.AccountsModule.Application.Services;
public class TokenValidationParametersFactory
{
    private readonly OptionsTokens _tokenOptions;

    public TokenValidationParametersFactory(IOptions<OptionsTokens> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public TokenValidationParameters Create(bool validateLifeTime = true)
        => Create(_tokenOptions, validateLifeTime);

    public static TokenValidationParameters Create(OptionsTokens _tokenOptions, bool validateLifetime = true)
    {
        return new TokenValidationParameters
        {
            ValidIssuer = _tokenOptions.Issuer,
            ValidateIssuer = true,

            ValidAudience = _tokenOptions.Audience,
            ValidateAudience = false,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key)),
            ValidateIssuerSigningKey = true,

            ValidateLifetime = validateLifetime,

            ClockSkew = TimeSpan.Zero,
        };
    }

}
