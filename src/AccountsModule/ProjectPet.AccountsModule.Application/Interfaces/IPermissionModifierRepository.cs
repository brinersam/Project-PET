using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Interfaces;
public interface IPermissionModifierRepository
{
    Task<Result<Guid, Error>> AddAsync(PermissionModifier permissionModifier, CancellationToken cancellation = default);
}
