using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Infrastructure.Database;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.Infrastructure.Repositories;
public class VolunteerRequestRepository : IVolunteerRequestRepository
{
    private readonly WriteDbContext _dbContext;
    public VolunteerRequestRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> AddAsync(VolunteerRequest volunteerReq, CancellationToken cancellationToken = default)
    {
        await _dbContext.VolunteerRequests.AddAsync(volunteerReq, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteerReq.Id;
    }

    public async Task<Result<Guid, Error>> Delete(VolunteerRequest volunteerReq, CancellationToken cancellationToken = default)
    {
        _dbContext.VolunteerRequests.Remove(volunteerReq);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteerReq.Id;
    }

    public async Task<Result<VolunteerRequest, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.VolunteerRequests
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (result is null)
            return Errors.General.NotFound(typeof(VolunteerRequest), id);

        return result;
    }

    public async Task<Result<Guid, Error>> Save(VolunteerRequest volunteerReq, CancellationToken cancellationToken = default)
    {
        _dbContext.VolunteerRequests.Attach(volunteerReq);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteerReq.Id;
    }
}
