namespace ProjectPet.VolunteerModule.Contracts.Requests;

public record GetVolunteerPaginatedRequest(
    int Page,
    int Take);