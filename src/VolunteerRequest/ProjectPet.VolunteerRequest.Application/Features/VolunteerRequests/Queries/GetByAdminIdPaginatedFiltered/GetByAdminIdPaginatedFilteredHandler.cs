using CSharpFunctionalExtensions;
using ProjectPet.Core.Extensions;
using ProjectPet.Core.ResponseModels;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Contracts.Dto;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByAdminIdPaginatedFiltered;
public class GetByAdminIdPaginatedFilteredHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetByAdminIdPaginatedFilteredHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerRequestDto>, Error>> HandleAsync(
        GetByAdminIdPaginatedFilteredQuery query,
        CancellationToken cancellationToken)
    {
        var dbQuery = _readDbContext.VolunteerRequests
            .Where(x => Equals(x.AdminId, query.AdminId))
            .AsQueryable();

        var statusFilter = query.Filters.Status;
        statusFilter ??= VolunteerRequestStatusDto.onReview;

        dbQuery = dbQuery.NullableWhere(statusFilter, x => x.Status == statusFilter);

        return await dbQuery.ToPagedListAsync(query, cancellationToken);
    }
}
