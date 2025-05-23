using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.DiscussionsModule.Infrastructure.Database;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Infrastructure.Repositories;
public class DiscussionsRepository : IDiscussionsRepository
{
    private readonly WriteDbContext _dbContext;
    public DiscussionsRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> AddAsync(Discussion discussion, CancellationToken cancellationToken = default)
    {
        await _dbContext.Discussions.AddAsync(discussion, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return discussion.Id;
    }

    public async Task<Result<Guid, Error>> Delete(Discussion discussion, CancellationToken cancellationToken = default)
    {
        _dbContext.Discussions.Remove(discussion);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return discussion.Id;
    }

    public async Task<Result<Discussion, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Discussions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (result is null)
            return Errors.General.NotFound(typeof(Discussion), id);

        return result;
    }

    public async Task<Result<Guid, Error>> Save(Discussion discussion, CancellationToken cancellationToken = default)
    {
        _dbContext.Discussions.Attach(discussion);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return discussion.Id;
    }
}
