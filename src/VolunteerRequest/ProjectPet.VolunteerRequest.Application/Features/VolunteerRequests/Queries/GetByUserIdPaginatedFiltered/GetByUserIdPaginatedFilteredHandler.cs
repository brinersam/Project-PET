using CSharpFunctionalExtensions;
using ProjectPet.Core.Extensions;
using ProjectPet.Core.ResponseModels;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Contracts.Dto;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByUserIdPaginatedFiltered;
public class GetByUserIdPaginatedFilteredHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetByUserIdPaginatedFilteredHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerRequestDto>, Error>> HandleAsync(
        GetByUserIdPaginatedFilteredQuery query,
        CancellationToken cancellationToken)
    {
        var dbQuery = _readDbContext.VolunteerRequests
            .Where(x => Equals(x.UserId, query.UserId))
            .AsQueryable();

        dbQuery = dbQuery.NullableWhere(query.Filters.Status, x => x.Status == query.Filters.Status);

        return await dbQuery.ToPagedListAsync(query, cancellationToken);
    }
}
