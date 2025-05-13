using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Contracts;
public interface IAccountsModuleContract
{
    Task<Result<Guid, Error>> CreatePermissionModifierAsync(
        Guid userId,
        PermissionModifierDto modifierDto,
        CancellationToken ctoken = default);
}
