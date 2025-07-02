using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Core.Extensions;
using ProjectPet.Core.ResponseModels;
using ProjectPet.FileService.Contracts;
using ProjectPet.FileService.Contracts.Dtos;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Contracts;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Responses;
using System.Linq.Expressions;

namespace ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;

public class GetPetsPaginatedHandler
{
    private readonly IReadDbContext _readDbContext;
    private readonly IFileService _fileService;
    private readonly ILogger<GetPetsPaginatedHandler> _logger;
    private readonly ISpeciesContract _speciesContract;


    public GetPetsPaginatedHandler(
        IReadDbContext readDbContext,
        IFileService fileService,
        ILogger<GetPetsPaginatedHandler> logger,
        ISpeciesContract speciesContract)
    {
        _readDbContext = readDbContext;
        _fileService = fileService;
        _logger = logger;
        _speciesContract = speciesContract;
    }

    public async Task<Result<PagedList<PetResponse>, Error>> HandleAsync(GetPetsPaginatedQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _readDbContext.Pets.AsQueryable();

        dbQuery = await ApplyFiltersAsync(dbQuery, _speciesContract, query, cancellationToken);

        dbQuery = ApplySorting(dbQuery, query);

        var petPagedList = await dbQuery.ToPagedListAsync(query, cancellationToken);

        var petResponsesResults = await Task.WhenAll(petPagedList.Data.Select(async x => await ToPetResponse(x, cancellationToken)));

        return new PagedList<PetResponse>
        {
            Data = petResponsesResults,
            PageIndex = petPagedList.PageIndex,
            PageSize = petPagedList.PageSize,
            TotalCount = petPagedList.TotalCount,
        };
    }

    private async Task<PetResponse> ToPetResponse(PetDto pet, CancellationToken cancellationToken = default)
    {
        var photoResponseList = await _fileService.PresignedUrlsDownloadAsync(
                new(
                    pet.Photos
                        .Select(x => new FileLocationDto(x.FileId, x.BucketName))
                        .ToList()
                ),
                cancellationToken
            );

        if (photoResponseList.IsFailure)
        {
            _logger.LogError("Failed to retrieve photos for pet (id: {p1}) with error {p2}", pet.Id, photoResponseList.Error);
            return PetResponse.FromRequest(pet, []);
        }

        var urls = photoResponseList.Value.Urls.ToDictionary(x => x.FileId, x => x.Url);
        var photoResponses = pet.Photos.Select(x => new PetPhotoResponse(x.FileName, x.FileId, urls[x.FileId]));

        return PetResponse.FromRequest(pet, photoResponses);
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