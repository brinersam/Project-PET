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
    private readonly IAccountRepository _accountRepository;

    public LoginHandler(
        UserManager<User> userManager,
        ILogger<LoginHandler> logger,
        ITokenProvider tokenProvider,
        IAccountRepository accountRepository)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
        _accountRepository = accountRepository;
    }

    public async Task<Result<LoginResponse, Error>> HandleAsync(LoginCommand cmd, CancellationToken cancellationToken = default)
    {
        var userRes = await _accountRepository.GetByEmailWRolesAsync(cmd.Email, cancellationToken);
        if (userRes.IsFailure)
            return userRes.Error;
        var user = userRes.Value;

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, cmd.Password);
        if (isPasswordValid == false)
            return Error.Validation("invalid.credentials", $"Invalid credentials");

        _logger.LogInformation($"User {user.Id} successfully logged in!");

        var session = await _tokenProvider.GenerateSessionAsync(user, cancellationToken);
        if (session.IsFailure)
            return session.Error;

        return session.Value with { Roles = user.Roles.Select(x => x.Name!).ToList() };
    }
}
