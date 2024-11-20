using CSharpFunctionalExtensions;
using ProjectPet.Application.Dto;
using ProjectPet.Application.Extensions;
using ProjectPet.Application.Models;
using ProjectPet.Domain.Shared;
using ProjectPet.VolunteerModule.Application.Interfaces;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Queries.GetVolunteers;

public class GetVolunteerPaginatedHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteerPaginatedHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerDto>, Error>> HandleAsync(
        GetVolunteerPaginatedQuery query,
        CancellationToken cancellationToken = default)
    {
        var volunteerQuery = _readDbContext.Volunteers;
        var volunteers = await volunteerQuery.ToPagedListAsync(query, cancellationToken);
        return volunteers;
    }
}
