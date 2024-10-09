using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers
{
    public class DeleteVolunteerHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly ILogger<DeleteVolunteerHandler> _logger;

        public DeleteVolunteerHandler(
            IVolunteerRepository volunteerRepository,
            ILogger<DeleteVolunteerHandler> logger)
        {
            _volunteerRepository = volunteerRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, Error>> HandleAsync(
            DeleteVolunteerRequest request,
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

            volunteerRes.Value.SetIsDeletedFlag(true);

            var result = await _volunteerRepository.Delete(volunteerRes.Value, cancellationToken);

            _logger.LogInformation("Volunteer with id {id} was deleted successfully!", request.Id);

            return result.Value;
        }
    }
}
