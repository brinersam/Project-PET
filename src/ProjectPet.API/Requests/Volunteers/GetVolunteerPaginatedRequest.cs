using ProjectPet.API.Etc;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetVolunteers;

namespace ProjectPet.API.Requests.Volunteers;

public record GetVolunteerPaginatedRequest(
    int Page,
    int Take) : IToCommand<GetVolunteerPaginatedQuery>
{
    public GetVolunteerPaginatedQuery ToCommand()
        => new() { Page = Page, RecordAmount = Take };
}
