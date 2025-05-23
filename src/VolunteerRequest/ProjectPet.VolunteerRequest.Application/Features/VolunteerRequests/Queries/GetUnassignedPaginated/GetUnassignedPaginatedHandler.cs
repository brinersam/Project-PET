using CSharpFunctionalExtensions;
using ProjectPet.Core.Extensions;
using ProjectPet.Core.HelperModels;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Contracts.Dto;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetUnassignedPaginated;
public class GetUnassignedPaginatedHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetUnassignedPaginatedHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerRequestDto>, Error>> HandleAsync(
        GetUnassignedPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var dbQuery = _readDbContext.VolunteerRequests
            .Where(x => Equals(x.AdminId, Guid.Empty))
            .Where(x => x.Status == VolunteerRequestStatusDto.submitted)
            .AsQueryable();

        return await dbQuery.ToPagedListAsync(query, cancellationToken);
    }
}
