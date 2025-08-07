using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.Core.Options;
using ProjectPet.SharedKernel.ErrorClasses;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using tempShared.Framework.Authorization;

namespace ProjectPet.AccountsModule.Application.Services;
public class TokenManager : ITokenProvider, ITokenRefresher, ITokenClaimsAccessor
{
    private readonly OptionsTokens _options;
    private readonly IAuthRepository _authRepository;
    private readonly TokenValidationParametersFactory _tokenValidationParametersFactory;

    public TokenManager(
        IOptions<OptionsTokens> options,
        IAuthRepository authRepository,
        TokenValidationParametersFactory tokenValidationParametersFactory)
    {
        _options = options.Value;
        _authRepository = authRepository;
        _tokenValidationParametersFactory = tokenValidationParametersFactory;
    }

    public AccessTokenWJti GenerateJwtAccessToken(User user)
    {
        var jti = Guid.NewGuid();

        var userRoleClaims = user.Roles
            .Select(role => new Claim(CustomClaims.ROLE, role.Name ?? "UNKNOWN_ROLE_NAME"));

        var userPermissionClaims = user.Roles
            .SelectMany(role => role.RolePermissions.Select(y => y.Permission.Code))
            .Distinct()
            .Select(code => new Claim(CustomClaims.PERMISSION, code));

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new ArgumentException($"Null email got through to {typeof(TokenManager)}")),
            new Claim(CustomClaims.ID, user.Id.ToString()),
            .. userRoleClaims,
            .. userPermissionClaims,
        ];

        var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var signCredts = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessExpiresMin),
            signingCredentials: signCredts,
            claims: claims);

        var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new(stringToken, jti);
    }

    public async Task<Guid> GenerateRefreshTokenAsync(User user, Guid jti, CancellationToken cancellationToken = default)
    {
        var session = new RefreshSession
        {
            RefreshToken = Guid.NewGuid(),
            Jti = jti,
            CreatedAt = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddMinutes(_options.RefreshExpiresMin),
            User = user,
        };

        await _authRepository.AddRefreshSessionAsync(session, cancellationToken);

        return session.RefreshToken;
    }

    public async Task<Result<IReadOnlyList<Claim>, Error>> GetTokenClaims(
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var validationResult = await jwtHandler
            .ValidateTokenAsync(accessToken, _tokenValidationParametersFactory.Create(validateLifeTime: false));

        if (validationResult.IsValid == false)
            return Error.Validation("invalid.token", "Invalid token!");

        return validationResult.ClaimsIdentity.Claims.ToList().AsReadOnly();
    }

    public async Task<Result<LoginResponse, Error>> RefreshTokens(
        Guid refreshToken,
        CancellationToken cancellationToken = default)
    {
        var session = await _authRepository.GetRefreshSessionAsync(refreshToken, cancellationToken);
        if (session is null)
            return Error.Validation("invalid.session", "Invalid session!");

        if (session.ExpiresIn < DateTime.UtcNow)
            return Error.Validation("invalid.session", "Session has expired!");

        await _authRepository.DeleteSessionAsync(session, cancellationToken);

        return await GenerateSessionAsync(session.User, cancellationToken);
    }

    public async Task<Result<LoginResponse, Error>> GenerateSessionAsync(User user, CancellationToken cancellationToken)
    {
        var newAccessToken = GenerateJwtAccessToken(user);
        var newRefreshToken = await GenerateRefreshTokenAsync(user, newAccessToken.jti, cancellationToken);
        return new LoginResponse(newRefreshToken, newAccessToken.accessToken, []);
    }
}
