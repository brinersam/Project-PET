using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Contracts;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.SharedKernel.ValueObjects;

namespace ProjectPet.AccountsModule.Application;
public class AccountsModuleContractImplementation : IAccountsModuleContract
{
    private readonly IPermissionModifierRepository _permissionModRepository;
    private readonly IAccountRepository _accountRepository;

    public AccountsModuleContractImplementation(
        IPermissionModifierRepository permissionModifier,
        IAccountRepository accountRepository)
    {
        _permissionModRepository = permissionModifier;
        _accountRepository = accountRepository;
    }

    public async Task<UnitResult<Error>> AddVolunteerDataToUserAsync(Guid userId,
                                                                VolunteerAccountDto accountDto,
                                                                CancellationToken ctoken = default)
    {
        var getUserRes = await _accountRepository.GetByIdAsync(userId, ctoken);
        if (getUserRes.IsFailure)
            return getUserRes.Error;

        var data = new VolunteerAccount(
            accountDto.PaymentInfos.Select(x => new PaymentInfo(x.Title, x.Instructions)),
            accountDto.Experience,
            accountDto.Certifications);

        var setVolunteerRes = getUserRes.Value.SetVolunteerData(data);
        if (setVolunteerRes.IsFailure)
            return setVolunteerRes.Error;

        return Result.Success<Error>();
    }

    public async Task<Result<Guid, Error>> CreatePermissionModifierAsync(Guid userId,
                                                                         PermissionModifierDto modifierDto,
                                                                         CancellationToken ctoken = default)
    {
        var getUserRes = await _accountRepository.GetByIdAsync(userId, ctoken);
        if (getUserRes.IsFailure)
            return getUserRes.Error;

        var createModifierRes = PermissionModifier.Create(
            userId,
            modifierDto.Code,
            modifierDto.IsAllowed,
            modifierDto.ExpiresAt);

        if (createModifierRes.IsFailure)
            return createModifierRes.Error;

        var addRes = await _permissionModRepository.AddAsync(createModifierRes.Value, ctoken);
        if (addRes.IsFailure)
            return addRes.Error;

        return addRes.Value;
    }
}
