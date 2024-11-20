using ProjectPet.Application.UseCases.Volunteers.Queries.GetVolunteers;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

public record GetVolunteerPaginatedRequest(
    int Page,
    int Take) : IToCommand<GetVolunteerPaginatedQuery>
{
    public GetVolunteerPaginatedQuery ToCommand()
        => new() { Page = Page, RecordAmount = Take };
}
