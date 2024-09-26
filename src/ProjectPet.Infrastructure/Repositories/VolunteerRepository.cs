using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.Application.UseCases.CreateVolunteer;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Infrastructure.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VolunteerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid, Error>> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id;
        }

        public async Task<Result<Volunteer, Error>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Volunteers
                .Include(x => x.OwnedPets)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result is null)
                return Errors.General.NotFound(typeof(Volunteer), id);

            return result;
        }

    }
}
