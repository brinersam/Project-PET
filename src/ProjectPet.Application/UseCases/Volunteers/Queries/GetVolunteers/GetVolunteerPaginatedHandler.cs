using CSharpFunctionalExtensions;
using ProjectPet.Application.Database;
using ProjectPet.Application.Dto;
using ProjectPet.Application.Extensions;
using ProjectPet.Application.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.Queries.GetVolunteers;

public class GetVolunteerPaginatedHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteerPaginatedHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerReadDto>, Error>> HandleAsync(
        GetVolunteerPaginatedQuery query,
        CancellationToken cancellationToken = default)
    {
        var volunteerQuery = _readDbContext.Volunteers;
        var volunteers = await volunteerQuery.ToPagedListAsync(query, cancellationToken);
        return volunteers;
    }
}
