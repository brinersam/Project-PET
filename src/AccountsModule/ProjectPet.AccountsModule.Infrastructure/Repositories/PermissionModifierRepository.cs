using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Infrastructure.Repositories;
public class PermissionModifierRepository : IPermissionModifierRepository
{
    private readonly AuthDbContext _dbContext;

    public PermissionModifierRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> AddAsync(PermissionModifier modifier, CancellationToken cancellationToken = default)
    {
        await _dbContext.PermissionModifiers.AddAsync(modifier, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return modifier.Id;
    }
}
