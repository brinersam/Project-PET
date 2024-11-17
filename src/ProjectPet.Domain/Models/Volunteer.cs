using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models.DDD;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models;

public class Volunteer : EntityBase, ISoftDeletable
{
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int YOExperience { get; private set; }
    public Phonenumber Phonenumber { get; private set; } = null!;
    private List<Pet> _ownedPets = null!;
    public IReadOnlyList<Pet> OwnedPets => _ownedPets;
    public PaymentMethodsList? PaymentMethods { get; private set; }
    public SocialNetworkList? SocialNetworks { get; private set; }
#pragma warning disable IDE0052 // Remove unread private members
    private bool _isDeleted = false;
#pragma warning restore IDE0052 // Remove unread private members

    public Volunteer(Guid id) : base(id) { }

    private Volunteer(
        Guid id,
        string fullName,
        string email,
        string description,
        int yOExperience,
        Phonenumber phoneNumber,
        IEnumerable<PaymentInfo> paymentMethods,
        IEnumerable<SocialNetwork> socialNetworks) : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        YOExperience = yOExperience;
        Phonenumber = phoneNumber;
        _ownedPets = [];
        PaymentMethods = new() { Data = paymentMethods.ToList() };
        SocialNetworks = new() { Data = socialNetworks.ToList() };
    }

    public static Result<Volunteer, Error> Create
        (
        Guid id,
        string fullName,
        string email,
        string description,
        int yOExperience,
        Phonenumber phoneNumber,
        IEnumerable<PaymentInfo> paymentMethods,
        IEnumerable<SocialNetwork> socialNetworks)
    {
        var resultID = Validator
            .ValidatorNull<Guid>()
            .Check(id, nameof(id));

        if (resultID.IsFailure)
            return resultID.Error;

        var validator = Validator.ValidatorString(Constants.STRING_LEN_MEDIUM);

        var result = validator.Check(fullName, nameof(fullName));
        if (result.IsFailure)
            return result.Error;

        result = validator.Check(email, nameof(email));
        if (result.IsFailure)
            return result.Error;

        result = validator.Check(description, nameof(description));
        if (result.IsFailure)
            return result.Error;

        return new Volunteer
            (
                id,
                fullName,
                email,
                description,
                yOExperience,
                phoneNumber,
                paymentMethods,
                socialNetworks
            );
    }

    public int PetsHoused() => _ownedPets.Count(x => x.Status == PetStatus.Home_Found);
    public int PetsLookingForHome() => _ownedPets.Count(x => x.Status == PetStatus.Looking_For_Home);
    public int PetsInCare() => _ownedPets.Count(x => x.Status == PetStatus.Requires_Care);

    public void UpdateGeneralInfo(string? FullName,
        string? Email,
        string? Description,
        int? YOExperience,
        Phonenumber? PhoneNumber)
    {
        if (!String.IsNullOrEmpty(FullName))
            this.FullName = FullName;

        if (!String.IsNullOrEmpty(Email))
            this.Email = Email;

        if (!String.IsNullOrEmpty(Description))
            this.Description = Description ?? this.Description;

        this.YOExperience = YOExperience ?? this.YOExperience;
        this.Phonenumber = PhoneNumber ?? this.Phonenumber;
    }

    public void Delete()
    {
        _isDeleted = true;
        foreach (var pet in _ownedPets)
            pet.Delete();
    }

    public void Restore()
    {
        _isDeleted = false;
        foreach (var pet in _ownedPets)
            pet.Restore();
    }

    public void UpdatePaymentMethods(IEnumerable<PaymentInfo> infos)
    {
        this.PaymentMethods = new() { Data = infos.ToList() };
    }

    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> infos)
    {
        this.SocialNetworks = new() { Data = infos.ToList() };
    }

    public void AddPet(Pet pet)
    {
        _ownedPets.Add(pet);
    }

    public Result<Pet, Error> GetPetById(Guid id)
    {
        Pet? pet = _ownedPets.FirstOrDefault(p => p.Id == id);
        if (pet is null)
            return Error.NotFound("record.not.found", $"No pet with id \"{id}\" was found for user {FullName}!");
        return pet;
    }

    public void SetPetPositionToFront(int x)
        => SetPetPosition(x, int.MinValue);

    public void SetPetPositionToBack(int x)
        => SetPetPosition(x, int.MaxValue);

    public void SetPetPosition(int initialPos, int targetPos)
    {
        // cap both positions to a range of 1 to ownedPets count
        int minPosition = 1;
        int maxPosition = _ownedPets.Count;

        initialPos = Math.Max(initialPos, minPosition);
        targetPos = Math.Max(targetPos, minPosition);

        initialPos = Math.Min(initialPos, maxPosition);
        targetPos = Math.Min(targetPos, maxPosition);

        if (initialPos == targetPos)
            return;

        var movedPet = _ownedPets.First(x => x.OrderingPosition.Value == initialPos);

        var petsToMove = _ownedPets
            .Where(pet =>
                   pet.OrderingPosition.Value >= Math.Min(initialPos, targetPos)
                && Math.Max(initialPos, targetPos) >= pet.OrderingPosition.Value
                && ReferenceEquals(pet, movedPet) == false)
            .ToArray();

        movedPet.SetPosition(targetPos);

        if (initialPos > targetPos)
        {
            foreach (var pet in petsToMove)
                pet.MovePositionForward();
        }

        else if (targetPos > initialPos)
        {
            foreach (var pet in petsToMove)
                pet.MovePositionBackwards();
        }
    }

    public UnitResult<Error> SetPetStatus(Guid petId, PetStatus status)
    {
        var petRes = GetPetById(petId);
        if (petRes.IsFailure)
            return petRes.Error;
        var pet = petRes.Value;

        pet.SetPetStatus(status);
        return Result.Success<Error>();
    }

    public UnitResult<Error> DeletePhotos(Guid petId, string[] photoPaths)
    {
        var petRes = GetPetById(petId);
        if (petRes.IsFailure)
            return petRes.Error;
        var pet = petRes.Value;

        pet.DeletePhotos(photoPaths);
        return Result.Success<Error>();
    }

    public UnitResult<Error> PatchPet(
        Guid petId,
        string? name,
        AnimalData? animalData,
        string? description,
        string? coat,
        HealthInfo? healthInfo,
        Address? address,
        Phonenumber? phonenumber)
    {
        var petRes = GetPetById(petId);
        if (petRes.IsFailure)
            return petRes.Error;
        var pet = petRes.Value;

        pet.PatchInfo(
            name,
            animalData,
            description,
            coat,
            healthInfo,
            address,
            phonenumber);

        return Result.Success<Error>();
    }
}

public record SocialNetworkList
{
    public List<SocialNetwork> Data { get; set; } = null!;
}
