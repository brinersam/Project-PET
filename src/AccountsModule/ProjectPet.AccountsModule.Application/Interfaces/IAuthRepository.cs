using ProjectPet.AccountsModule.Domain;

namespace ProjectPet.AccountsModule.Application.Interfaces;

public interface IAuthRepository
{
    Task AddRefreshSessionAsync(RefreshSession session, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(RefreshSession session, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(Guid refreshToken, CancellationToken cancellationToken = default);
    Task<bool> DoesUserHavePermissionCodeAsync(Guid userID, string permissionCode, CancellationToken cancellationToken = default);
    Task<RefreshSession?> GetRefreshSessionAsync(Guid refreshToken, CancellationToken cancellationToken = default);
}