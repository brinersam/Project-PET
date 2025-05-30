using ProjectPet.Core.Requests;
using ProjectPet.Core.ResponseModels;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetUnassignedPaginated;
public record GetUnassignedPaginatedQuery :
    PaginatedQueryBase,
    IMapFromRequest<GetUnassignedPaginatedQuery, GetVolunteerRequestsPaginatedRequest>
{
    public static GetUnassignedPaginatedQuery FromRequest(GetVolunteerRequestsPaginatedRequest request)
        => new() { Page = request.Page, RecordAmount = request.Take };
}
