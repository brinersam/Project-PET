using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.Core.Abstractions;
using ProjectPet.Core.HelperModels;
using ProjectPet.Core.Extensions;
using ProjectPet.SharedKernel.Dto;
using ProjectPet.SharedKernel.ErrorClasses;
using System.Linq.Expressions;

namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;

public class GetPetsPaginatedHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetPetsPaginatedHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<PetDto>, Error>> HandleAsync(GetPetsPaginatedQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _readDbContext.Pets.AsQueryable();

        dbQuery = await ApplyFiltersAsync(dbQuery, query, _readDbContext, cancellationToken);

        dbQuery = ApplySorting(dbQuery, query);

        return await dbQuery.ToPagedListAsync(query, cancellationToken);
    }

    private IQueryable<PetDto> ApplySorting(
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

    private async Task<IQueryable<PetDto>> ApplyFiltersAsync(
        IQueryable<PetDto> dbQuery,
        GetPetsPaginatedQuery query,
        IReadDbContext _readDbContext,
        CancellationToken cancellationToken = default)
    {
        if (query.Filters is null)
            return dbQuery;

        Guid? speciesNameId = await SpeciesNameToId(query.Filters.SpeciesName, _readDbContext, cancellationToken);
        Guid? breedNameId = await BreedNameToId(query.Filters.BreedName, _readDbContext, cancellationToken);

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
        IReadDbContext _readDbContext,
        CancellationToken cancellationToken)
    {
        if (name is null)
            return null;

        return await _readDbContext.Breeds
            .Where(x => x.Value.Contains(name))
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static async Task<Guid?> SpeciesNameToId(
        string? name,
        IReadDbContext _readDbContext,
        CancellationToken cancellationToken)
    {
        if (name is null)
            return null;

        return await _readDbContext.Species
            .Where(x => x.Name.Contains(name))
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}