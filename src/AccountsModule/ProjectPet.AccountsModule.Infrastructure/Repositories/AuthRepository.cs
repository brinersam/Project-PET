using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.Framework.Authorization;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Infrastructure.Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _authDbContext;

    public AuthRepository(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }

    public Result<Permission, Error> GetPermissionsForRole(Guid value)
    {
        return new Permission() { Code = PermissionCodes.SpeciesAccess };
    }
}
