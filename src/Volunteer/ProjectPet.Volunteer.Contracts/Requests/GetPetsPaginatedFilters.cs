namespace ProjectPet.VolunteerModule.Contracts.Requests;
public record GetPetsPaginatedFilters(
    Guid? VolunteerId,
    string? Name,
    int? Age,
    string? SpeciesName,
    string? BreedName,
    string? Coat);
