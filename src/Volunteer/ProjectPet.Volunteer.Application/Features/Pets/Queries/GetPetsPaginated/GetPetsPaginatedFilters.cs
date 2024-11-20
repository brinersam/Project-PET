namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;
public record GetPetsPaginatedFilters(
    Guid? VolunteerId,
    string? Name,
    int? Age,
    string? SpeciesName,
    string? BreedName,
    string? Coat);
