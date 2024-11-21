using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.Core.HelperModels;
using ProjectPet.SharedKernel.ErrorClasses;
using System.Linq.Expressions;

namespace ProjectPet.Core.Extensions;
public static class Extensions
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

    public static IQueryable<TType> NullableWhere<TType, TParam>(
        this IQueryable<TType> query,
        TParam value,
        Expression<Func<TType, bool>> expression)
    {
        return value is null ? query : query.Where(expression);
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
