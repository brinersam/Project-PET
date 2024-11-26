using Microsoft.AspNetCore.Mvc;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Login;
using ProjectPet.AccountsModule.Application.Features.Auth.Commands.Register;
using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Framework;

namespace ProjectPet.AccountsModule.Presentation;

public class AccountController : CustomControllerBase
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

        return Ok(result.Value);
    }
}
