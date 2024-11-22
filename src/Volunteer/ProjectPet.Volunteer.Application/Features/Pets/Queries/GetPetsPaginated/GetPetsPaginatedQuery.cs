using ProjectPet.Core.Abstractions;
using ProjectPet.Core.HelperModels;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;

public record GetPetsPaginatedQuery(GetPetsPaginatedFilters? Filters, GetPetsPaginatedSorting? Sorting)
    : PaginatedQueryBase, IMapFromRequest<GetPetsPaginatedQuery, GetPetsPaginatedRequest>
{
    public static GetPetsPaginatedQuery FromRequest(GetPetsPaginatedRequest req)
    {
        return new GetPetsPaginatedQuery(req.Filters, req.Sorting) { Page = req.Page, RecordAmount = req.Take };
    }
}
