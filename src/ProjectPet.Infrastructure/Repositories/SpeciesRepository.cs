﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.Application.UseCases.AnimalSpecies;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SpeciesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> AddAsync(Species species, CancellationToken cancellationToken = default)
    {
        await _dbContext.Species.AddAsync(species, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return species.Id;
    }

    public async Task<Result<Species, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Species
            .Include(x => x.RelatedBreeds)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (result is null)
            return Errors.General.NotFound(typeof(Species), id);

        return result;
    }

    public Result<Guid, Error> Update(Species species, CancellationToken cancellationToken = default)
    {
        _dbContext.Species.Update(species);
        return species.Id;
    }

    public async Task<Result<Guid, Error>> Save(Species species, CancellationToken cancellationToken = default)
    {
        _dbContext.Species.Attach(species);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return species.Id;
    }

    public async Task<Result<Guid, Error>> DeleteAndSaveChangesasync(Species species, CancellationToken cancellationToken = default)
    {
        _dbContext.Species.Remove(species);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return species.Id;
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var trimmedName = name.Trim().ToLower();

        var result = await _dbContext.Species
            .AsNoTracking()
            .AnyAsync(x => x.Name.ToLower().Equals(trimmedName), cancellationToken);

        return result;
    }
}
