using ProjectPet.Application.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;

public record GetPetsPaginatedQuery(GetPetsPaginatedFilters? Filters, GetPetsPaginatedSorting? Sorting) : PaginatedQueryBase;
