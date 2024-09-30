using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public CreateVolunteerHandler(IVolunteerRepository volunteerRepository)
        {
            _volunteerRepository = volunteerRepository;
        }

        public async Task<Result<Guid,Error>> HandleAsync(
            CreateVolunteerRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var phoneNumberRes = PhoneNumber.Create(
                request.Phonenumber.Phonenumber,
                request.Phonenumber.PhonenumberAreaCode).Value;

            var volunteerRes = Volunteer.Create(
                Guid.NewGuid(),
                request.FullName,
                request.Description,
                request.Email,
                request.YOExperience,
                phoneNumberRes,
                request.OwnedPets,
                request.PaymentMethods,
                request.SocialNetworks);

            if (volunteerRes.IsFailure)
                return volunteerRes.Error;

            var addRes = await _volunteerRepository.AddAsync
                                    (volunteerRes.Value, cancellationToken);

            if (addRes.IsFailure)
                return addRes.Error;

            return volunteerRes.Value.Id; 
        }
    }
}
