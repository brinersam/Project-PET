using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Infrastructure.Repositories;
public class AccountRepository : IAccountRepository
{
    private readonly AuthDbContext _authDbContext;

    public AccountRepository(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }

    public async Task<Result<User, Error>> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _authDbContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId), cancellationToken);
        if (user == null)
            return Errors.General.NotFound(typeof(User));

        return user;
    }

    public async Task<Result<User, Error>> GetByIdWRolesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _authDbContext.Users
            .Include(x => x.Roles)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync(x => x.Id.Equals(userId), cancellationToken);

        if (user == null)
            return Errors.General.NotFound(typeof(User));

        return user;
    }

    public async Task<Result<User, Error>> GetByEmailWRolesAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _authDbContext.Users
            .Include(x => x.Roles)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(email.ToLower()), cancellationToken);

        if (user == null)
            return Errors.General.NotFound(typeof(User));

        return user;
    }

    public async Task Save(User user, CancellationToken cancellationToken)
    {
        _authDbContext.Attach(user);
        await _authDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<HashSet<PermissionModifier>> GetPermissionModifiersAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await _authDbContext.PermissionModifiers
            .Where(x => DateTime.UtcNow >= x.ExpiresAt)
            .ToHashSetAsync(cancellationToken);
        return result;
    }
}
