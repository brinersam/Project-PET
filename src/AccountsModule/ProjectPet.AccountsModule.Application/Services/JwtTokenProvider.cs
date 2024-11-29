using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.Core.Options;
using ProjectPet.Framework.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectPet.AccountsModule.Application.Services;
public class JwtTokenProvider : ITokenProvider
{
    private readonly OptionsJwt _options;

    public JwtTokenProvider(IOptions<OptionsJwt> options)
    {
        _options = options.Value;
    }

    public string GenerateJwtAccessToken(User user)
    {
        Claim[] claims =
        {
            new Claim("Role", user.RoleId.ToString()),
            //PermissionClaim.New(),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new ArgumentException($"Null email got through to {nameof(JwtTokenProvider)}"))
        };

        var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var signCredts = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresMin),
            signingCredentials: signCredts,
            claims: claims);

        var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return stringToken;
    }
}
