namespace ProjectPet.VolunteerRequests.Contracts.Requests;
public record GetVolunteerRequestFilteredPaginatedRequest(
    int Page,
    int Take,
    GetVolunteerRequestFilters Filters)
{ }
