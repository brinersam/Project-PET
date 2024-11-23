namespace ProjectPet.VolunteerModule.Contracts.Requests;

public record GetPetsPaginatedRequest(
    int Page,
    int Take,
    GetPetsPaginatedFilters? Filters,
    GetPetsPaginatedSorting? Sorting);

