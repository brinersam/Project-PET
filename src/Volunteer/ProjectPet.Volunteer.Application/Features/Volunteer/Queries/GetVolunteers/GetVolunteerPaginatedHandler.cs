using CSharpFunctionalExtensions;
using ProjectPet.Core.Abstractions;
using ProjectPet.Core.HelperModels;
using ProjectPet.Core.Extensions;
using ProjectPet.SharedKernel.Dto;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;

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
