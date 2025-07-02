using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.Entities.AbstractBase;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Domain.Models;

public class Pet : SoftDeletableEntityBase
{
    public Guid VolunteerId { get; private set; }
    public Position OrderingPosition { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public AnimalData AnimalData { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Coat { get; private set; } = null!;
    public HealthInfo HealthInfo { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public Phonenumber Phonenumber { get; private set; } = null!;
    public PetStatus Status { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public DateOnly CreatedOn { get; private set; }
    public IReadOnlyList<PetPhoto> Photos => _photos;
    private List<PetPhoto> _photos = [];
    public PaymentMethodsList PaymentMethods { get; private set; } = null!;
    public Pet() : base(Guid.Empty) { } //efcore

    private Pet(
        Guid id,
        Guid volunteerId,
        string name,
        AnimalData animalData,
        string description,
        string coat,
        Position orderingPosition,
        HealthInfo healthInfo,
        Address address,
        Phonenumber phoneNumber,
        PetStatus status,
        DateOnly dateOfBirth,
        DateOnly createdOn,
        IEnumerable<PetPhoto> photos,
        IEnumerable<PaymentInfo> paymentMethods) : base(id)
    {
        Name = name;
        AnimalData = animalData;
        Description = description;
        Coat = coat;
        OrderingPosition = orderingPosition;
        HealthInfo = healthInfo;
        Address = address;
        Phonenumber = phoneNumber;
        Status = status;
        DateOfBirth = dateOfBirth;
        CreatedOn = createdOn;
        _photos = photos.ToList();
        PaymentMethods = new() { Data = paymentMethods.ToList() };
    }

    public static Result<Pet, Error> Create(
        string name,
        AnimalData animalData,
        string description,
        string coat,
        Position orderingPosition,
        HealthInfo healthInfo,
        Address address,
        Phonenumber phoneNumber,
        PetStatus status,
        DateOnly dateOfBirth,
        IEnumerable<PetPhoto> photos,
        IEnumerable<PaymentInfo> paymentMethods)
    {
        DateOnly createdOn = DateOnly.FromDateTime(DateTime.Now);

        var validatorStr = Validator.ValidatorString();

        var result = validatorStr.Check(name, nameof(name));
        if (result.IsFailure)
            return result.Error;

        result = validatorStr
            .SetMaxLen(Constants.STRING_LEN_MEDIUM)
            .Check(description, nameof(description));

        if (result.IsFailure)
            return result.Error;

        if (status == PetStatus.NotSet)
            return Error.Validation("value.is.invalid", "Pet status must be set!");

        return new Pet(
            Guid.Empty,
            Guid.Empty,
            name,
            animalData,
            description,
            coat,
            orderingPosition,
            healthInfo,
            address,
            phoneNumber,
            status,
            dateOfBirth,
            createdOn,
            photos,
            paymentMethods);
    }

    public void AddPhotos(IEnumerable<PetPhoto> photos)
    {
        if (photos.Any() == false)
            return;

        if (_photos.Any(photo => photo.IsPrimary == true))
        {
            _photos.AddRange(photos);
            return;
        }

        List<PetPhoto> resultPhotos = photos.Skip(1).ToList();

        PetPhoto mainPhoto = photos.First().Duplicate(isPrimary: true);

        resultPhotos.Insert(0, mainPhoto);

        _photos = resultPhotos;
    }

    public void SetPetStatus(PetStatus status)
        => Status = status;

    public void SetPosition(Position position)
        => OrderingPosition = position;

    public void MovePositionForward(int amount = 1)
        => OrderingPosition = OrderingPosition.MoveForward(amount);

    public void MovePositionBackwards(int amount = 1)
        => OrderingPosition = OrderingPosition.MoveBackward(amount);

    public void DeletePhotos(IEnumerable<string> petFileIds)
    {
        bool primaryPhotoWasDeleted = false;
        bool deletedAtLeastOnePhoto = false;

        foreach (var photoFileId in petFileIds)
        {
            int photoToDeleteIdx = _photos.FindIndex(x => string.Equals(photoFileId, x.FileId));
            if (photoToDeleteIdx == -1)
                continue;

            var photoToDelete = _photos[photoToDeleteIdx];

            if (photoToDelete.IsPrimary)
                primaryPhotoWasDeleted = true;

            _photos.RemoveAt(photoToDeleteIdx);
            deletedAtLeastOnePhoto = true;
        }

        if (deletedAtLeastOnePhoto == false)
            return;

        if (primaryPhotoWasDeleted && _photos.Count > 0)
        {
            var firstPhotoMadePrimary = _photos[0].Duplicate(isPrimary: true);
            _photos[0] = firstPhotoMadePrimary;
        }

        _photos = _photos.Select(x => x.Duplicate()).ToList();
    }

    public void PatchInfo(
        string? name,
        AnimalData? animalData,
        string? description,
        string? coat,
        HealthInfo? healthInfo,
        Address? address,
        Phonenumber? phonenumber)
    {
        Name = name ?? Name;
        AnimalData = animalData ?? AnimalData;
        Description = description ?? Description;
        Coat = coat ?? Coat;
        HealthInfo = healthInfo ?? HealthInfo;
        Address = address ?? Address;
        Phonenumber = phonenumber ?? Phonenumber;
    }

    public UnitResult<Error> SetMainPhoto(string PhotoFileId)
    {
        var photoIdx = _photos.FindIndex(x => string.Equals(x.FileId, PhotoFileId));
        if (photoIdx == -1)
            return Errors.General.NotFound(typeof(PetPhoto));

        var oldMainPhoto = _photos.FindIndex(x => x.IsPrimary == true);
        if (oldMainPhoto != -1)
        {
            var oldPrimaryPhotoMadeRegular = _photos[oldMainPhoto].Duplicate(isPrimary: false);
            _photos[oldMainPhoto] = oldPrimaryPhotoMadeRegular;
        }

        var aPhotoMadePrimary = _photos[photoIdx].Duplicate(isPrimary: true);

        _photos.RemoveAt(photoIdx);
        _photos.Insert(0, aPhotoMadePrimary);

        _photos = _photos.Select(x => x.Duplicate()).ToList();

        return Result.Success<Error>();
    }
}

public record PaymentMethodsList
{
    public List<PaymentInfo> Data { get; set; } = null!;
}

public enum PetStatus
{
    NotSet,
    Requires_Care,
    Looking_For_Home,
    Home_Found,
}