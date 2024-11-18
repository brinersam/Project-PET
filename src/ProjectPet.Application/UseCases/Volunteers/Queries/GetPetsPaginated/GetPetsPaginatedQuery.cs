using ProjectPet.Application.Models;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetPetsPaginated;

namespace ProjectPet.Application.UseCases.Volunteers.Queries.GetPets;

public record GetPetsPaginatedQuery(GetPetsPaginatedFilters? Filters, GetPetsPaginatedSorting? Sorting) : PaginatedQueryBase;
