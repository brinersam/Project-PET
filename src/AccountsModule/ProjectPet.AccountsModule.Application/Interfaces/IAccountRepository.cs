using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Interfaces;
public interface IAccountRepository
{
    Task<Result<User, Error>> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Result<User, Error>> GetByEmailWRolesAsync(string email, CancellationToken cancellationToken);
    Task Save(User user, CancellationToken cancellationToken);
    Task<HashSet<PermissionModifier>> GetPermissionModifiersAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<User, Error>> GetByIdWRolesAsync(Guid userId, CancellationToken cancellationToken);
}
