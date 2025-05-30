using ProjectPet.Core.Requests;
using ProjectPet.Core.ResponseModels;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByAdminIdPaginatedFiltered;
public record GetByAdminIdPaginatedFilteredQuery(
    Guid AdminId,
    GetVolunteerRequestFilters Filters
    ) : PaginatedQueryBase, IMapFromRequest<GetByAdminIdPaginatedFilteredQuery, GetVolunteerRequestFilteredPaginatedRequest, Guid>
{
    public static GetByAdminIdPaginatedFilteredQuery FromRequest(GetVolunteerRequestFilteredPaginatedRequest request, Guid adminId)
        => new(adminId, request.Filters) { Page = request.Page, RecordAmount = request.Take };
}
