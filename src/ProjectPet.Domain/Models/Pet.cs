using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models.DDD;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models
{
    public class Pet : EntityBase, ISoftDeletable
    {
        private bool _isDeleted = false;
        public string Name { get; private set; } = null!;
        public AnimalData AnimalData { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string Coat { get; private set; } = null!;
        public HealthInfo HealthInfo { get; private set; } = null!;
        public Address Address { get; private set; } = null!;
        public PhoneNumber PhoneNumber { get; private set; } = null!;
        public Status Status { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public DateOnly CreatedOn { get; private set; }
        public PhotoList? Photos { get; private set; }
        public PaymentMethodsList? PaymentMethods { get; private set; }
        public Pet() : base(Guid.Empty) { } //efcore

        private Pet(
            Guid id,
            string name,
            AnimalData animalData,
            string description,
            string coat,
            HealthInfo healthInfo,
            Address address,
            PhoneNumber phoneNumber,
            Status status,
            DateOnly dateOfBirth,
            DateOnly createdOn,
            IEnumerable<PetPhoto> photos,
            IEnumerable<PaymentInfo> paymentMethods) : base(id)
        {
            Name = name;
            AnimalData = animalData;
            Description = description;
            Coat = coat;
            HealthInfo = healthInfo;
            Address = address;
            PhoneNumber = phoneNumber;
            Status = status;
            DateOfBirth = dateOfBirth;
            CreatedOn = createdOn;
            Photos = new() { Data = photos.ToList() };
            PaymentMethods = new() { Data = paymentMethods.ToList() };
        }

        public static Result<Pet,Error> Create(
            Guid id,
            string name,
            AnimalData animalData,
            string description,
            string coat,
            HealthInfo healthInfo,
            Address address,
            PhoneNumber phoneNumber,
            Status status,
            DateOnly dateOfBirth,
            DateOnly createdOn,
            IEnumerable<PetPhoto> photos,
            IEnumerable<PaymentInfo> paymentMethods)
        {
            var resultID = Validator.ValidatorNull<Guid>().Check(id,nameof(id));
            if (resultID.IsFailure)
                return resultID.Error;

            var validatorStr = Validator.ValidatorString();

            var result = validatorStr.Check(name,nameof(name));
            if (result.IsFailure)
                return result.Error;

            result = validatorStr
                .SetMaxLen(Constants.STRING_LEN_MEDIUM)
                .Check(description, nameof(description));

            if (result.IsFailure)
                return result.Error;

            if (status == Status.NotSet)
                return Error.Validation("value.is.invalid", "Pet status must be set!");

            return new Pet(
                id,
                name,
                animalData,
                description,
                coat,
                healthInfo,
                address,
                phoneNumber,
                status,
                dateOfBirth,
                createdOn,
                photos,
                paymentMethods);
        }

        public void Delete()
        {
            _isDeleted = true;
        }

        public void Restore()
        {
            _isDeleted = false;
        }
    }

    public record PaymentMethodsList
    {
        public List<PaymentInfo> Data { get; set; }
    }

    public record PhotoList
    {
        public List<PetPhoto> Data { get; set; }
    }

    public enum Status
    {
        NotSet,
        Requires_Care,
        Looking_For_Home,
        Home_Found
    }
}

