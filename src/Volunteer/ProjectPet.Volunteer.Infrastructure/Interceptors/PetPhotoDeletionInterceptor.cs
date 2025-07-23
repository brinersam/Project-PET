using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using ProjectPet.FileService.Contracts;
using ProjectPet.FileService.Contracts.Dtos;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Infrastructure.Interceptors;
public class PetPhotoDeletionInterceptor : SaveChangesInterceptor
{
    private readonly IFileService _fileService;
    private readonly ILogger<PetPhotoDeletionInterceptor> _logger;

    public PetPhotoDeletionInterceptor(
        IFileService fileService,
        ILogger<PetPhotoDeletionInterceptor> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var deletedPets = eventData.Context.ChangeTracker
            .Entries<Pet>()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in deletedPets)
        {
            var pet = entry.Entity;

            // i had an issue where for whatever reason pet wouldnt pull its owned jsonb type (photos) from db
            // now issue is not reproducing
            // if it ever happens again: reason is something related to lazy loading so just load it fully in here
            // dont modify the one above btw
            // .AsNoTracking() is also important
            var petWithPhotos = await eventData.Context.Set<Pet>().AsNoTracking().FirstAsync(x => x.Id == pet!.Id, cancellationToken);
            if (petWithPhotos.Photos.Count <= 0)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var photosToDelete = petWithPhotos.Photos.Select(x => new FileLocationDto(x.FileId, x.BucketName)).ToList();

            _logger.LogInformation(
                "Pet Deletion! Also deleting photos: {PhotosList}",
                String.Join(";\n", photosToDelete.Select(x => $"{x.BucketName}: Photo with id {x.FileId}")));

            await Task.WhenAll(
                photosToDelete.Select(async x =>
                     {
                         var deleteResult = await _fileService.DeleteFileAsync(x);
                         if (deleteResult.IsFailure)
                         {
                             _logger.LogWarning(
                                 "Failed to delete photo (id: {photoId}) that was owned by now deleted pet (id: {petId}): {error}",
                                 x.FileId,
                                 pet.Id,
                                 deleteResult.Error.Message
                             );
                         }

                         return deleteResult;
                     }
                )
            );
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
