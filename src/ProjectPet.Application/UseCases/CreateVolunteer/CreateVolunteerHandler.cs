using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly ILogger<CreateVolunteerHandler> _logger;

        public CreateVolunteerHandler(
            IVolunteerRepository volunteerRepository,
            ILogger<CreateVolunteerHandler> logger)
        {
            _volunteerRepository = volunteerRepository;
            _logger = logger;
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
                request.Email,
                request.Description,
                request.YOExperience,
                phoneNumberRes,
                request.OwnedPets,
                request.PaymentMethods,
                request.SocialNetworks);

            if (volunteerRes.IsFailure)
            {
                _logger.LogInformation("Failed to add a new volunteer {name}!\n {error}", request.FullName, volunteerRes.Error.Message);
                return volunteerRes.Error;
            }

            var addRes = await _volunteerRepository.AddAsync
                                    (volunteerRes.Value, cancellationToken);

            if (addRes.IsFailure)
            {
                _logger.LogInformation("Failed to add a new volunteer {name}!\n {error}", volunteerRes.Value.FullName, addRes.Error.Message);
                return addRes.Error;
            }
                

            _logger.LogInformation("Created volunteer {name} with id {id}", volunteerRes.Value.FullName, volunteerRes.Value.Id);

            return volunteerRes.Value.Id; 
        }
    }
}
