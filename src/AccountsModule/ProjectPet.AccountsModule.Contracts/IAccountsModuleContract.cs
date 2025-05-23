using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.AccountsModule.Contracts;
public interface IAccountsModuleContract
{
    Task<Result<Guid, Error>> CreatePermissionModifierAsync(
        Guid userId,
        PermissionModifierDto modifierDto,
        CancellationToken ctoken = default);

    Task<UnitResult<Error>> MakeUserVolunteerAsync(Guid userId,
        VolunteerAccountDto accountDto,
        CancellationToken ctoken = default);
}
