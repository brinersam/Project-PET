using Microsoft.EntityFrameworkCore;
using ProjectPet.Application.UseCases.CreateVolunteer;
using ProjectPet.Domain.Models;

namespace ProjectPet.Infrastructure.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VolunteerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id; // TODO result
        }

        public async Task<Volunteer> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Volunteers
                .Include(x => x.OwnedPets)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result == null || result.Equals(Guid.Empty))
                throw new ArgumentException("Id not found"); // TODO refactor to result

            return result;
        }

    }
}
