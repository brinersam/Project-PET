using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.ErrorClasses;
using System.Text.Json.Serialization;

namespace ProjectPet.VolunteerModule.Domain.Models;

public record PetPhoto
{
    public string FileId { get; init; }
    public string BucketName { get; init; }
    public string FileName { get; init; }
    public string ContentType { get; init; }
    public bool IsPrimary { get; init; }

    [JsonConstructor]
    private PetPhoto(
        string fileId,
        string bucketName,
        string fileName,
        string contentType,
        bool isPrimary)
    {
        FileId = fileId;
        BucketName = bucketName;
        FileName = fileName;
        ContentType = contentType;
        IsPrimary = isPrimary;
    }

    public static Result<PetPhoto, Error> Create(
        string fileId,
        string bucketName,
        string fileName,
        string contentType,
        bool isPrimary = false)
    {
        var stringValidator = Validator.ValidatorString(Constants.STRING_LEN_MEDIUM);

        var validationRes = stringValidator.Check(fileId, nameof(fileId));
        if (validationRes.IsFailure)
            return validationRes.Error;

        validationRes = stringValidator.Check(bucketName, nameof(bucketName));
        if (validationRes.IsFailure)
            return validationRes.Error;

        validationRes = stringValidator.Check(fileName, nameof(fileName));
        if (validationRes.IsFailure)
            return validationRes.Error;

        validationRes = stringValidator.Check(contentType, nameof(contentType));
        if (validationRes.IsFailure)
            return validationRes.Error;

        return new PetPhoto(fileId, bucketName, fileName, contentType, isPrimary);
    }

    public PetPhoto Duplicate(
        string? fileId = null,
        string? bucketName = null,
        string? fileName = null,
        string? contentType = null,
        bool? isPrimary = null)
    {
        return new PetPhoto(
            fileId ?? this.FileId,
            bucketName ?? this.BucketName,
            fileName ?? this.FileName,
            contentType ?? this.ContentType,
            isPrimary ?? this.IsPrimary);
    }
}
