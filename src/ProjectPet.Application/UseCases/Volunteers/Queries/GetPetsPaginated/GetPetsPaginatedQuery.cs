using ProjectPet.Application.Models;

namespace ProjectPet.Application.UseCases.Volunteers.Queries.GetPetsPaginated;

public record GetPetsPaginatedQuery(GetPetsPaginatedFilters? Filters, GetPetsPaginatedSorting? Sorting) : PaginatedQueryBase;
