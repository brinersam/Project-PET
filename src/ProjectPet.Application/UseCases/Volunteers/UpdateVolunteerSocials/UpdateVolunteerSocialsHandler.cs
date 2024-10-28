using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers;

public class UpdateVolunteerSocialsHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateVolunteerSocialsHandler> _logger;

    public UpdateVolunteerSocialsHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateVolunteerSocialsHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> HandleAsync(
        UpdateVolunteerSocialsRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;

        var SocialNetworksList = request.Dto.SocialNetworks
                                    .Select(x =>
                                        SocialNetwork.Create(x.Name, x.Link).Value)
                                    .ToList();

        volunteerRes.Value.UpdateSocialNetworks(SocialNetworksList);

        var saveRes = await _volunteerRepository.Save(volunteerRes.Value);
        if (saveRes.IsFailure)
            return saveRes.Error;

        _logger.LogInformation("Updated volunteer with id {id} successfully!", saveRes.Value);
        return saveRes.Value;
    }
}
