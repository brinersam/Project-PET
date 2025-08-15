using DEVShared;
using MassTransit;
using MediatR;
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
    private readonly IPublishEndpoint _publisher;

    public PetPhotoDeletionInterceptor(
        IFileService fileService,
        ILogger<PetPhotoDeletionInterceptor> logger,
        IPublishEndpoint publisher)
    {
        _fileService = fileService;
        _logger = logger;
        _publisher = publisher;
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

            _publisher.Publish(new PetDeletedEvent(pet.Id, photosToDelete), CancellationToken.None);
            _logger.LogInformation(
                "Pet Deletion! Also deleting photos: {PhotosList}",
                String.Join(";\n", photosToDelete.Select(x => $"{x.BucketName}: Photo with id {x.FileId}")));
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
