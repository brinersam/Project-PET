using CSharpFunctionalExtensions;
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
}
