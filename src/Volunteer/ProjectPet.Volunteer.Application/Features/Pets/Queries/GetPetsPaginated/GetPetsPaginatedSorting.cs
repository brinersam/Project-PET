namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;
public record GetPetsPaginatedSorting(
    bool? VolunteerId,
    bool? Name,
    bool? Age,
    bool? Coat,
    bool SortAsc = false)
{
    public IEnumerable<(string key, bool? value)> Properties()
    {
        yield return (nameof(VolunteerId), VolunteerId);
        yield return (nameof(Name), Name);
        yield return (nameof(Age), Age);
        yield return (nameof(Coat), Coat);
    }
};
