using ProjectPet.Domain.Models.DDD;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectPet.Domain.Models
{
    public class Pet : Entity
    {
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
        public Pet() : base(Guid.Empty) {} //efcore

        public Pet(
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
            Photos = new() {Data = photos.ToList() };
            PaymentMethods = new() { Data = paymentMethods.ToList() };
        }

        public static Pet Create(
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
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException("Argument id can not be empty!");

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Argument name can not be empty!");

            if (String.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("Argument description can not be empty!");

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
    }

    public record PaymentMethodsList
    {
        public List<PaymentInfo> Data { get; set; }
    }

    public record PhotoList
    {
        public List<PetPhoto> Data { get;  set; }
    }

    public enum Status
    {
        NotSet,
        Requires_Care,
        Looking_For_Home,
        Home_Found
    }
}

