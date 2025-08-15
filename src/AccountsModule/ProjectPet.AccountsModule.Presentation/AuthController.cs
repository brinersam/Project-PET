using Microsoft.AspNetCore.Mvc;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Login;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Logout;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.RefreshTokens;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Register;
using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Framework;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Presentation;

public class AuthController : CustomControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegisterHandler handler,
        CancellationToken cancellationToken = default)
    {
        var cmd = RegisterCommand.FromRequest(request);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken = default)
    {
        var cmd = LoginCommand.FromRequest(request);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        HttpContext.Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString());

        return Ok(result.Value with { RefreshToken = default });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    [FromServices] LogoutHandler handler,
    CancellationToken cancellationToken = default)
    {
        var successfulTokenParse = Guid.TryParse(HttpContext.Request.Cookies["refreshToken"], out Guid refreshToken);
        HttpContext.Response.Cookies.Append("refreshToken", String.Empty);

        if (!successfulTokenParse)
            return Ok();

        var cmd = new LogoutCommand(refreshToken);
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPost("refresh-tokens")]
    public async Task<IActionResult> RefreshTokens(
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken = default)
    {
        var refreshTokenRaw = HttpContext.Request.Cookies["refreshToken"];

        if (!Guid.TryParse(refreshTokenRaw, out var refreshToken))
            return Ok();// Errors.General.ValueIsInvalid("refreshtoken", "refreshToken").ToResponse();

        var cmd = new RefreshTokensCommand(refreshToken);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        HttpContext.Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString());

        return Ok(result.Value with { RefreshToken = default});
    }
}

