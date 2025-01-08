using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Infrastructure.Database;

namespace ProjectPet.VolunteerModule.Infrastructure.Repositories;

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

    public async Task<Result<Guid, Error>> SoftDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Attach(volunteer);
        volunteer.SoftDelete();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }
}
