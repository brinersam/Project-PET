using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Contracts;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.SharedKernel.ValueObjects;
using System.Data;

namespace ProjectPet.AccountsModule.Application;
public class AccountsModuleContractImplementation : IAccountsModuleContract
{
    private readonly IPermissionModifierRepository _permissionModRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AccountsModuleContractImplementation> _logger;

    public AccountsModuleContractImplementation(
        IPermissionModifierRepository permissionModifier,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        ILogger<AccountsModuleContractImplementation> logger)
    {
        _permissionModRepository = permissionModifier;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> MakeUserVolunteerAsync(Guid userId,
                                                                VolunteerAccountDto accountDto,
                                                                CancellationToken ctoken = default)
    {
        var getUserRes = await _accountRepository.GetByIdAsync(userId, ctoken);
        if (getUserRes.IsFailure)
            return getUserRes.Error;

        var transaction = await _unitOfWork.BeginTransactionAsync(ctoken);

        var data = new VolunteerAccount(
            accountDto.PaymentInfos.Select(x => new PaymentInfo(x.Title, x.Instructions)),
            accountDto.Experience,
            accountDto.Certifications);

        var setVolunteerRes = getUserRes.Value.SetVolunteerData(data);
        if (setVolunteerRes.IsFailure)
            return setVolunteerRes.Error;

        var roleResult = await _userManager.AddToRoleAsync(getUserRes.Value, VolunteerAccount.ROLENAME);
        if (roleResult.Succeeded == false)
            return roleResult.Errors.Select(x => Error.Failure(x.Code, x.Description)).FirstOrDefault()!;

        transaction.Commit();
        _logger.LogInformation("User (id {O1}) got role: {O2}", userId, VolunteerAccount.ROLENAME);

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

        var restrictedString = modifierDto.IsAllowed ? "given" : "restricted from";
        var logString = "User (id {O1}) was " + $"{restrictedString}" + " a permission {O2} until {O3}";

        _logger.LogInformation(
            logString,
            userId,
            modifierDto.Code,
            modifierDto.ExpiresAt);

        return addRes.Value;
    }
}
