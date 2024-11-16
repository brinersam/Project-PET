﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.Application.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.Extensions;
public static class Ext
{
    public static bool IsDelegateFailed<T>(out T result, out Error error, Result<T, Error> factoryRes)
    {
        if (factoryRes.IsFailure)
        {
            result = default!;
            error = factoryRes.Error;
            return true;
        }
        else
        {
            result = factoryRes.Value;
            error = null!;
            return false;
        }
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> queryable,
        PaginatedQueryBase query,
        CancellationToken cancellationToken = default
    )
        => await queryable.ToPagedListAsync(query.Skip, query.RecordAmount, query.Page, cancellationToken);

    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> queryable,
        int skip,
        int take,
        int page,
        CancellationToken cancellationToken = default)
    {
        var count = await queryable.CountAsync(cancellationToken);

        var data = await queryable.Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new PagedList<T>()
        {
            Data = data,
            PageIndex = page,
            PageSize = take,
            TotalCount = count
        };
    }
}
