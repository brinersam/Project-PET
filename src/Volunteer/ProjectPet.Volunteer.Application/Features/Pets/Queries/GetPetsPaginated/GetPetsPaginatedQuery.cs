using ProjectPet.Core.HelperModels;

namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;

public record GetPetsPaginatedQuery(GetPetsPaginatedFilters? Filters, GetPetsPaginatedSorting? Sorting) : PaginatedQueryBase;
