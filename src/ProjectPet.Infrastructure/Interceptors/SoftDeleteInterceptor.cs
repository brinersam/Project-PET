﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Infrastructure.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            if (entry.Entity is ISoftDeletable deletableEntry)
            {
                entry.State = EntityState.Modified;
                deletableEntry.Delete();
            }

        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
