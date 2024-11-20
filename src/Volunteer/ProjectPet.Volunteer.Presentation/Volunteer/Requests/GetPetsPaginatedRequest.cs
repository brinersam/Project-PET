using ProjectPet.Application.UseCases.Volunteers.Queries.GetPetsPaginated;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

public record GetPetsPaginatedRequest(
    int Page,
    int Take,
    GetPetsPaginatedFilters? Filters,
    GetPetsPaginatedSorting? Sorting) : IToCommand<GetPetsPaginatedQuery>
{
    public GetPetsPaginatedQuery ToCommand()
        => new GetPetsPaginatedQuery(Filters, Sorting) { Page = Page, RecordAmount = Take };
}
