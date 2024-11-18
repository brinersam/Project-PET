namespace ProjectPet.Application.UseCases.Volunteers.Queries.GetPetsPaginated;
public record GetPetsPaginatedFilters(
    Guid? VolunteerId,
    string? Name,
    int? Age,
    string? SpeciesName,
    string? BreedName,
    string? Coat);
