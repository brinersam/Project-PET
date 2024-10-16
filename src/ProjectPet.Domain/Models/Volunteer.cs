﻿using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models.DDD;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models
{
    public class Volunteer : EntityBase, ISoftDeletable
    {
        public string FullName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public int YOExperience { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; } = null!;
        private List<Pet> _ownedPets;
        public IReadOnlyList<Pet> OwnedPets => _ownedPets;
        public PaymentMethodsList? PaymentMethods { get; private set; }
        public SocialNetworkList? SocialNetworks { get; private set; }
        private bool _isDeleted = false;

        public Volunteer(Guid id) : base(id) { }

        private Volunteer(
            Guid id,
            string fullName,
            string email,
            string description,
            int yOExperience,
            PhoneNumber phoneNumber,
            IEnumerable<Pet> ownedPets,
            IEnumerable<PaymentInfo> paymentMethods,
            IEnumerable<SocialNetwork> socialNetworks) : base(id)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            YOExperience = yOExperience;
            PhoneNumber = phoneNumber;
            _ownedPets = ownedPets.ToList();
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
            PhoneNumber phoneNumber,
            IEnumerable<Pet> ownedPets,
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
                    ownedPets,
                    paymentMethods,
                    socialNetworks
                );
        }

        public int PetsHoused() => _ownedPets.Count(x => x.Status == Status.Home_Found);
        public int PetsLookingForHome() => _ownedPets.Count(x => x.Status == Status.Looking_For_Home);
        public int PetsInCare() => _ownedPets.Count(x => x.Status == Status.Requires_Care);

        public void UpdateGeneralInfo(string? FullName,
            string? Email,
            string? Description,
            int? YOExperience,
            PhoneNumber? PhoneNumber)
        {
            if (!String.IsNullOrEmpty(FullName))
                this.FullName = FullName;

            if (!String.IsNullOrEmpty(Email))
                this.Email = Email;

            if (!String.IsNullOrEmpty(Description))
                this.Description = Description ?? this.Description;

            this.YOExperience = YOExperience ?? this.YOExperience;
            this.PhoneNumber = PhoneNumber ?? this.PhoneNumber;
        }

        public void Delete()
        {
            _isDeleted = true;
        }

        public void Restore()
        {
            _isDeleted = false;
        }

        public void UpdatePaymentMethods(IEnumerable<PaymentInfo> infos)
        {
            this.PaymentMethods = new() { Data = infos.ToList() };
        }

        public void UpdateSocialNetworks(IEnumerable<SocialNetwork> infos)
        {
            this.SocialNetworks = new() { Data = infos.ToList() };
        }
    }
    public record SocialNetworkList
    {
        public List<SocialNetwork> Data { get; set; }
    }
}
