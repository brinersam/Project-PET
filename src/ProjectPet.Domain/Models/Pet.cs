using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models.DDD;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models;

public class Pet : EntityBase, ISoftDeletable
{
#pragma warning disable IDE0052 // Remove unread private members
    private bool _isDeleted = false;
#pragma warning restore IDE0052 // Remove unread private members
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
    public PhotoList Photos { get; private set; } = null!;
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
        Photos = new() { Data = photos.ToList() };
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

        if (Photos.Data.Any(photo => photo.IsPrimary == true))
        {
            Photos.Data.AddRange(photos);
            return;
        }

        List<PetPhoto> resultPhotos = photos.Skip(1).ToList();

        PetPhoto mainPhoto = PetPhoto.Create(photos.First().StoragePath, true).Value;

        resultPhotos.Insert(0, mainPhoto);

        Photos.Data.AddRange(resultPhotos);
    }

    public void Delete()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }

    public void SetPetStatus(PetStatus status)
        => Status = status;

    public void SetPosition(Position position)
        => OrderingPosition = position;

    public void MovePositionForward(int amount = 1)
        => OrderingPosition = OrderingPosition.MoveForward(amount);

    public void MovePositionBackwards(int amount = 1)
        => OrderingPosition = OrderingPosition.MoveBackward(amount);

    public void DeletePhotos(string[] photoPaths)
    {
        bool primaryPhotoWasDeleted = false;
        bool deletedAtLeastOnePhoto = false;

        var data = Photos.Data;
        foreach (var path in photoPaths)
        {
            int photoToDeleteIdx = data.FindIndex(x => String.Equals(path.ToLower(), x.StoragePath.ToLower()));
            if (photoToDeleteIdx == -1)
                continue;

            var photoToDelete = data[photoToDeleteIdx];

            if (photoToDelete.IsPrimary)
                primaryPhotoWasDeleted = true;

            data.RemoveAt(photoToDeleteIdx);
            deletedAtLeastOnePhoto = true;
        }

        if (deletedAtLeastOnePhoto == false)
            return;

        if (primaryPhotoWasDeleted && data.Count > 0)
        {
            var firstPhotoMadePrimary = PetPhoto.Create(data[0].StoragePath, true).Value;
            data[0] = firstPhotoMadePrimary;
        }

        var deepCopyData = data.Select(x => PetPhoto.Create(x.StoragePath, x.IsPrimary).Value).ToList();

        Photos = new PhotoList() { Data = deepCopyData };
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
}

public record PaymentMethodsList
{
    public List<PaymentInfo> Data { get; set; } = null!;
}

public record PhotoList
{
    public List<PetPhoto> Data { get; set; } = null!;
}

public enum PetStatus
{
    NotSet,
    Requires_Care,
    Looking_For_Home,
    Home_Found
}

