using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;

namespace ProjectPet.AccountsModule.Infrastructure.Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _authDbContext;
    private readonly UserManager<User> _userManager;

    public AuthRepository(
        AuthDbContext authDbContext,
        UserManager<User> userManager)
    {
        _authDbContext = authDbContext;
        _userManager = userManager;
    }

    public async Task<bool> DoesUserHavePermissionCodeAsync(Guid userID, string permissionCode)
    {
        var user = await _authDbContext.Users.FirstOrDefaultAsync(u => u.Id == userID);

        var roles = await _userManager.GetRolesAsync(user);

        var roleIds = _authDbContext.Roles.Where(r => roles.Contains(r.Name)).Select(x => x.Id).ToHashSet();

        var permissionExists = await _authDbContext.RolePermissions.AnyAsync
            (
                rp => 
                    roleIds.Contains(rp.RoleId) && 
                    Equals(rp.Permission.Code,permissionCode) 
            );
            
        return permissionExists;
    }
}
