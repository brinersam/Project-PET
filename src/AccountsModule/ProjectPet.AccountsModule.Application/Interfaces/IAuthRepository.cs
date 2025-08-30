using ProjectPet.AccountsModule.Domain;
using ProjectPet.Framework.Authorization;

namespace ProjectPet.AccountsModule.Application.Interfaces;

public interface IAuthRepository
{
    Task AddRefreshSessionAsync(RefreshSession session, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(RefreshSession session, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(Guid refreshToken, CancellationToken cancellationToken = default);
    Task<bool> DoesUserHavePermissionCodeAsync(Guid userID, string permissionCode, CancellationToken cancellationToken = default);
    bool DoesUserHavePermissionCode(UserScopedData userData, string permissionCode);
    Task<RefreshSession?> GetRefreshSessionAsync(Guid refreshToken, CancellationToken cancellationToken = default);
}