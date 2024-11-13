using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.Application.Repositories;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;
using ProjectPet.Infrastructure.DbContexts;

namespace ProjectPet.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly WriteDbContext _dbContext;

    public VolunteerRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Volunteers
            .Include(x => x.OwnedPets)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (result is null)
            return Errors.General.NotFound(typeof(Volunteer), id);

        return result;
    }

    public async Task<Result<Guid, Error>> Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Attach(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Remove(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }
}
