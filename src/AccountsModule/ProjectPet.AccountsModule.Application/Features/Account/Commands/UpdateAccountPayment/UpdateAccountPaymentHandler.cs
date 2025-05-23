using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.ValueObjects;

namespace ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountPayment;

public class UpdateAccountPaymentHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<UpdateAccountPaymentHandler> _logger;

    public UpdateAccountPaymentHandler(
        IAccountRepository accountRepository,
        ILogger<UpdateAccountPaymentHandler> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }
    public async Task<UnitResult<Error>> HandleAsync(
        UpdateAccountPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        var userRes = await _accountRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (userRes.IsFailure)
            return userRes.Error;
        var user = userRes.Value;

        user.UpdatePaymentMethods(command.PaymentInfos
            .Select(x => new PaymentInfo(x.Instructions, x.Title)));

        await _accountRepository.Save(user, cancellationToken);

        _logger.LogInformation("Updated account with id {id} successfully!", user.Id);
        return Result.Success<Error>();
    }
}
