using Microsoft.EntityFrameworkCore;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Infrastructure.Database;

namespace ProjectPet.AccountsModule.Infrastructure.Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _authDbContext;

    public AuthRepository(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
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

    public async Task DeleteSessionAsync(Guid refreshToken, CancellationToken cancellationToken = default)
    {
        var session = await _authDbContext.RefreshSessions.FirstOrDefaultAsync(x => x.RefreshToken.Equals(refreshToken), cancellationToken);
        if (session is null)
            return;

        _authDbContext.RefreshSessions.Remove(session);
        await _authDbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task AddRefreshSessionAsync(RefreshSession session, CancellationToken cancellationToken = default)
    {
        _authDbContext.RefreshSessions.Add(session);
        await _authDbContext.SaveChangesAsync(cancellationToken);
    }
}