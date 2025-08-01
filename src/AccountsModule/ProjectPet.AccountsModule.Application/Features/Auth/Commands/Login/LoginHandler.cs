using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Contracts.Dto;
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

    public async Task<Result<LoginResponse, Error>> HandleAsync(LoginCommand cmd, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(cmd.Email);
        if (user == null)
            return Error.Validation("user.doesnt.exists", $"User does not exist");

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, cmd.Password);
        if (isPasswordValid == false)
            return Error.Validation("invalid.credentials", $"Invalid credentials");

        var userRoles = await _userManager.GetRolesAsync(user!);


        _logger.LogInformation($"User {user.Id} successfully logged in!");

        var session = await _tokenProvider.GenerateSessionAsync(user, cancellationToken);
        if (session.IsFailure)
            return session.Error;

        return session.Value with { Roles = userRoles.ToList()};
    }
}
