using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Services;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.Login;

public class LoginHandler
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<LoginHandler> _logger;
    private readonly ITokenProvider _tokenProvider;

    public LoginHandler(
        UserManager<User> userManager,
        ILogger<LoginHandler> logger,
        ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string, Error>> HandleAsync(LoginCommand cmd, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(cmd.Email);
        if (user == null)
            return Error.Validation("user.doesnt.exists", $"User does not exist");

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, cmd.Password);
        if (isPasswordValid == false)
            return Error.Validation("invalid.credentials", $"Invalid credentials");

        var token = _tokenProvider.GenerateJwtAccessToken(user);

        _logger.LogInformation($"User {user.Id} successfully logged in!");

        return token;
    }
}
