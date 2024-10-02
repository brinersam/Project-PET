using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers
{
    public class UpdateVolunteerInfoHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly ILogger<CreateVolunteerHandler> _logger;

        public UpdateVolunteerInfoHandler(
            IVolunteerRepository volunteerRepository,
            ILogger<CreateVolunteerHandler> logger)
        {
            _volunteerRepository = volunteerRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> HandleAsync(
            UpdateVolunteerInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            var volunteerRes = await _volunteerRepository.GetAsync(request.Id, cancellationToken);
            if (volunteerRes.IsFailure)
            {
                _logger.LogInformation("Failed to get volunteer with id: {id}!\n {error}",
                    request.Id,
                    volunteerRes.Error.Message);

                return volunteerRes.Error;
            }

            PhoneNumber number = null!;
            if (request.Dto.PhoneNumber != null)
            {
                number = PhoneNumber.Create(
                    request.Dto.PhoneNumber.Phonenumber,
                    request.Dto.PhoneNumber.PhonenumberAreaCode).Value;
            }

            volunteerRes.Value.UpdateGeneralInfo(
                request.Dto.FullName,
                request.Dto.Email,
                request.Dto.Description,
                request.Dto.YOExperience,
                number);

            var saveRes = await _volunteerRepository.Save(volunteerRes.Value);
            if (saveRes.IsFailure)
            {
                _logger.LogInformation("Failed to save database after modifying a volunteer with id:{id}!\n {error}",
                    volunteerRes.Value.Id,
                    saveRes.Error.Message);

                return saveRes.Error;
            }
                

            _logger.LogInformation("Updated volunteer with id {id} successfully!", saveRes.Value);
            return saveRes.Value;
        }
    }
}
