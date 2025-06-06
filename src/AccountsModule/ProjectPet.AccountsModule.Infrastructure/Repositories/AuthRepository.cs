﻿using Microsoft.AspNetCore.Identity;
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

    public async Task<RefreshSession?> GetRefreshSessionAsync(Guid refreshToken, CancellationToken cancellationToken = default)
        => await _authDbContext.RefreshSessions
        .Include(x => x.User)
        .FirstOrDefaultAsync(x => x.RefreshToken.Equals(refreshToken), cancellationToken);

    public async Task DeleteSessionAsync(RefreshSession session, CancellationToken cancellationToken = default)
    {
        _authDbContext.RefreshSessions.Remove(session);
        await _authDbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task AddRefreshSessionAsync(RefreshSession session, CancellationToken cancellationToken = default)
    {
        _authDbContext.RefreshSessions.Add(session);
        await _authDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DoesUserHavePermissionCodeAsync(Guid userID, string permissionCode, CancellationToken cancellationToken = default)
    {
        var user = await _authDbContext.Users.FirstOrDefaultAsync(u => u.Id == userID, cancellationToken);

        if (IsUserHasPermissionModifier(userID, permissionCode, out bool IsPermitted))
            return IsPermitted;

        var roles = await _userManager.GetRolesAsync(user!);
        var roleIds = _authDbContext.Roles.Where(r => roles.Contains(r.Name)).Select(x => x.Id).ToHashSet();

        var permissionExists = await _authDbContext.RolePermissions.AnyAsync
            (
                rp =>
                    roleIds.Contains(rp.RoleId) &&
                    Equals(rp.Permission.Code, permissionCode),
                cancellationToken
            );

        return permissionExists;
    }

    private bool IsUserHasPermissionModifier(Guid userID, string code, out bool isPermitted)
    {
        isPermitted = false;

        var modifier = _authDbContext.PermissionModifiers
            .Where(x => DateTime.UtcNow >= x.ExpiresAt)
            .FirstOrDefault(x => x.UserId == userID && Equals(x.Code, code));

        var modifierExists = modifier is not null;

        if (modifierExists)
            isPermitted = modifier.IsAllowed;

        return modifierExists;
    }
}
