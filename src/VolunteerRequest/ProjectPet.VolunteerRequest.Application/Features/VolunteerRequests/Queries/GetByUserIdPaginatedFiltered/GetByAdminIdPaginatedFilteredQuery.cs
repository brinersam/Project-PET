using ProjectPet.Core.Abstractions;
using ProjectPet.Core.HelperModels;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByUserIdPaginatedFiltered;
public record GetByUserIdPaginatedFilteredQuery(
    Guid UserId,
    GetVolunteerRequestFilters Filters
    ) : PaginatedQueryBase, IMapFromRequest<GetByUserIdPaginatedFilteredQuery, GetVolunteerRequestFilteredPaginatedRequest, Guid>
{
    public static GetByUserIdPaginatedFilteredQuery FromRequest(GetVolunteerRequestFilteredPaginatedRequest request, Guid UserId)
        => new(UserId, request.Filters) { Page = request.Page, RecordAmount = request.Take };
}
