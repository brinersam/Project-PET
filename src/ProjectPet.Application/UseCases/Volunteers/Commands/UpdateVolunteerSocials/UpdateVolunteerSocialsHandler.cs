using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Repositories;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerSocials;

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
        UpdateVolunteerSocialsCommand request,
        CancellationToken cancellationToken = default)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;

        var SocialNetworksList = request.SocialNetworks
                                    .Select(x =>
                                        SocialNetwork.Create(x.Name, x.Link).Value)
                                    .ToList();

        volunteerRes.Value.UpdateSocialNetworks(SocialNetworksList);

        var saveRes = await _volunteerRepository.Save(volunteerRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        _logger.LogInformation("Updated volunteer with id {id} successfully!", saveRes.Value);
        return saveRes.Value;
    }
}
