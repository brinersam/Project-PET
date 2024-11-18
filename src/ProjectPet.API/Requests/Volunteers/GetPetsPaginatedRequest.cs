using ProjectPet.API.Etc;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetPets;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetPetsPaginated;

namespace ProjectPet.API.Requests.Volunteers;

public record GetPetsPaginatedRequest(
    int Page,
    int Take,
    GetPetsPaginatedFilters? Filters,
    GetPetsPaginatedSorting? Sorting) : IToCommand<GetPetsPaginatedQuery>
{
    public GetPetsPaginatedQuery ToCommand()
        => new GetPetsPaginatedQuery(Filters, Sorting) { Page = Page, RecordAmount = Take };
}
