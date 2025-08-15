using CSharpFunctionalExtensions;
using DEVShared;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.Exceptions;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.SharedKernel.ValueObjects;

namespace ProjectPet.AccountsModule.Infrastructure.EventConsumers.VolunteerRequestApproved;
public class UpgradeAccountToVolunteer : IConsumer<VolunteerRequestApprovedEvent>
{
    private readonly IAccountRepository _accountRepository;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UpgradeAccountToVolunteer> _logger;

    public UpgradeAccountToVolunteer(
        IAccountRepository accountRepository,
        UserManager<User> userManager,
        ILogger<UpgradeAccountToVolunteer> logger)
    {
        _accountRepository = accountRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<VolunteerRequestApprovedEvent> context)
    {
        var message = context.Message;
        var userId = message.userId;
        var accountDto = message.accountDto;
        try
        {
            await Handler(userId, accountDto);
        }
        catch (IntegrationEventErroredException ex)
        {
            _logger.LogError(ex, "Failure during integration event consumer! User (id {p1}) failed to get role: {p2}", userId, VolunteerAccount.ROLENAME);
            return;
        }

        _logger.LogInformation("User (id {O1}) got role: {O2}", userId, VolunteerAccount.ROLENAME);
    }

    private async Task Handler(Guid userId, VolunteerAccountDto accountDto)
    {
        var getUserRes = await _accountRepository.GetByIdAsync(userId, CancellationToken.None);
        if (getUserRes.IsFailure)
            throw new IntegrationEventErroredException(getUserRes.Error.Message);

        var data = new VolunteerAccount(
            accountDto.PaymentInfos.Select(x => new PaymentInfo(x.Title, x.Instructions)),
            accountDto.Experience,
            accountDto.Certifications);

        var setVolunteerRes = getUserRes.Value.SetVolunteerData(data);
        if (setVolunteerRes.IsFailure)
            throw new IntegrationEventErroredException(setVolunteerRes.Error.Message);

        var roleResult = await _userManager.AddToRoleAsync(getUserRes.Value, VolunteerAccount.ROLENAME);
        if (roleResult.Succeeded == false)
        {
            throw new IntegrationEventErroredException(
                roleResult.Errors
                    .Select(
                        x => Error.Failure(x.Code, x.Description))
                                    .FirstOrDefault()?
                                    .Message
                                    ??
                                    "Error while adding user to a role volunteer"
            );
        }

        await _accountRepository.Save(getUserRes.Value, CancellationToken.None);

    }
}
