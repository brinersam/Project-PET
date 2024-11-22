using CSharpFunctionalExtensions;
using ProjectPet.Core.HelperModels;
using ProjectPet.Core.Extensions;
using ProjectPet.SharedKernel.ErrorClasses;
using System.Linq.Expressions;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.SpeciesModule.Contracts;

namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;

public class GetPetsPaginatedHandler
{
    private readonly IReadDbContext _readDbContext;
    private readonly ISpeciesContract _speciesContract;


    public GetPetsPaginatedHandler(
        IReadDbContext readDbContext,
        ISpeciesContract speciesContract)
    {
        _readDbContext = readDbContext;
        _speciesContract = speciesContract;
    }

    public async Task<Result<PagedList<PetDto>, Error>> HandleAsync(GetPetsPaginatedQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _readDbContext.Pets.AsQueryable();

        dbQuery = await ApplyFiltersAsync(dbQuery, _speciesContract, query, cancellationToken);

        dbQuery = ApplySorting(dbQuery, query);

        return await dbQuery.ToPagedListAsync(query, cancellationToken);
    }

    private static IQueryable<PetDto> ApplySorting(
        IQueryable<PetDto> dbQuery,
        GetPetsPaginatedQuery query)
    {
        if (query.Sorting is null)
            return dbQuery;

        var sortingValue = query.Sorting
            .Properties()
            .FirstOrDefault(x => x.value == true);

        if (sortingValue.value == false)
            return dbQuery;

        Expression<Func<PetDto, object>> key = sortingValue.key switch
        {
            "VolunteerId" => (x) => x.VolunteerId,
            "Name" => (x) => x.Name,
            "Age" => (x) => x.DateOfBirth,
            "Coat" => (x) => x.Coat
            // todo maybe rewrite to dapper so we can use SpeciesName and BreedName for sorting
        };

        dbQuery = query.Sorting.SortAsc switch
        {
            true => dbQuery.OrderBy(key),
            false => dbQuery.OrderByDescending(key)
        };

        return dbQuery;
    }

    private async static Task<IQueryable<PetDto>> ApplyFiltersAsync(
        IQueryable<PetDto> dbQuery,
        ISpeciesContract speciesContract,
        GetPetsPaginatedQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.Filters is null)
            return dbQuery;

        Guid? speciesNameId = await SpeciesNameToId(query.Filters.SpeciesName, speciesContract, cancellationToken);
        Guid? breedNameId = await BreedNameToId(query.Filters.BreedName, speciesContract, cancellationToken);

        query.Filters.Deconstruct(
            out Guid? VolunteerId,
            out string? Name,
            out int? Age,
            out string? SpeciesName,
            out string? BreedName,
            out string? Coat);

        dbQuery = dbQuery.NullableWhere(VolunteerId, x => x.VolunteerId == VolunteerId);
        dbQuery = dbQuery.NullableWhere(Name, x => x.Name.Contains(Name!));
        dbQuery = dbQuery.NullableWhere(Coat, x => x.Coat == Coat);
        dbQuery = dbQuery.NullableWhere(Age, x => DateOnly.FromDateTime(DateTime.Now).Year - x.DateOfBirth.Year == Age);

        if (speciesNameId is not null)
            dbQuery = dbQuery.NullableWhere(speciesNameId, x => x.SpeciesID == speciesNameId);

        if (breedNameId is not null)
            dbQuery = dbQuery.NullableWhere(breedNameId, x => x.BreedID == breedNameId);

        return dbQuery;
    }

    private static async Task<Guid?> BreedNameToId(
        string? name,
        ISpeciesContract speciesContract,
        CancellationToken cancellationToken)
    {
        if (name is null)
            return null;

        var breedRes = await speciesContract.GetBreedByNameAsync(name, cancellationToken);
        if (breedRes.IsFailure)
            return Guid.Empty;

        return breedRes.Value.Id;
    }

    private static async Task<Guid?> SpeciesNameToId(
        string? name,
        ISpeciesContract speciesContract,
        CancellationToken cancellationToken)
    {
        if (name is null)
            return null;

        var speciesRes = await speciesContract.GetSpeciesByNameAsync(name, cancellationToken);
        if (speciesRes.IsFailure)
            return Guid.Empty;

        return speciesRes.Value.Id;
    }
}