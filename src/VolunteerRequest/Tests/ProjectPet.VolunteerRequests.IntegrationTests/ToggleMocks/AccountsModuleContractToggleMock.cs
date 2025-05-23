using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Application;
using ProjectPet.AccountsModule.Contracts;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.VolunteerRequests.IntegrationTests.ToggleMocks;
public class AccountsModuleContractToggleMock : ToggleMockBase<IAccountsModuleContract, AccountsModuleContractImplementation>, IAccountsModuleContract
{
    public async Task<Result<Guid, Error>> CreatePermissionModifierAsync(Guid userId, PermissionModifierDto modifierDto, CancellationToken ctoken = default)
    {
        if (IsMocked(nameof(CreatePermissionModifierAsync)))
            return await Mock.CreatePermissionModifierAsync(userId, modifierDto, ctoken);
        else
            return await CreateInstance().CreatePermissionModifierAsync(userId, modifierDto, ctoken);
    }

    public async Task<UnitResult<Error>> MakeUserVolunteerAsync(Guid userId, VolunteerAccountDto accountDto, CancellationToken ctoken = default)
    {
        if (IsMocked(nameof(MakeUserVolunteerAsync)))
            return await Mock.MakeUserVolunteerAsync(userId, accountDto, ctoken);
        else
            return await CreateInstance().MakeUserVolunteerAsync(userId, accountDto, ctoken);
    }
}
