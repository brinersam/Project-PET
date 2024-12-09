using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain.UserData;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountSocials;

public class UpdateAccountSocialsHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<UpdateAccountSocialsHandler> _logger;

    public UpdateAccountSocialsHandler(
        IAccountRepository accountRepository,
        ILogger<UpdateAccountSocialsHandler> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }
    public async Task<UnitResult<Error>> HandleAsync(
        UpdateAccountSocialsCommand command,
        CancellationToken cancellationToken = default)
    {
        var userRes = await _accountRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (userRes.IsFailure)
            return userRes.Error;
        var user = userRes.Value;

        user.UpdateSocialNetworks(command.SocialNetworks
            .Select(x => new SocialNetwork(x.Name, x.Link)));

        await _accountRepository.Save(user, cancellationToken);

        _logger.LogInformation("Updated account with id {id} successfully!", user.Id);
        return Result.Success<Error>();
    }
}
