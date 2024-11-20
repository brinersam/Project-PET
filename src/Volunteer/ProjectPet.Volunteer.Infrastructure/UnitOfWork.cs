﻿using Microsoft.EntityFrameworkCore.Storage;
using ProjectPet.Application.Database;
using ProjectPet.VolunteerModule.Infrastructure.Database;
using System.Data;

namespace ProjectPet.VolunteerModule.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _context;

    public UnitOfWork(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
