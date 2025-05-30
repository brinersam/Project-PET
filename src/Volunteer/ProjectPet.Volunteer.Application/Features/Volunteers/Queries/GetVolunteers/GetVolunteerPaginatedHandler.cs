using CSharpFunctionalExtensions;
using ProjectPet.Core.Extensions;
using ProjectPet.Core.ResponseModels;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Queries.GetVolunteers;

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
