using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models;

public record PetPhoto
{
    public string StoragePath { get; } = null!;
    public bool IsPrimary { get; }

    private PetPhoto(string storagePath, bool isPrimary)
    {
        StoragePath = storagePath;
        IsPrimary = isPrimary;
    }

    public static Result<PetPhoto, Error> Create(string storagePath, bool isPrimary = false)
    {
        var result = Validator
            .ValidatorString(Constants.STRING_LEN_MEDIUM)
            .Check(storagePath, nameof(storagePath)); ;

        if (result.IsFailure)
            return result.Error;

        return new PetPhoto(storagePath, isPrimary);
    }
}
