using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.CloseDiscussion;
public class CloseDiscussionHandler
{
    private readonly IDiscussionsRepository _discussionRepository;
    private readonly ILogger<CloseDiscussionHandler> _logger;

    public CloseDiscussionHandler(
        IDiscussionsRepository requestRepository,
        ILogger<CloseDiscussionHandler> logger)
    {
        _discussionRepository = requestRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        CloseDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        var getRes = await _discussionRepository.GetByIdAsync(command.DiscussionId, cancellationToken);
        if (getRes.IsFailure)
            return getRes.Error;

        var endDiscussionRes = getRes.Value.EndDiscussion();
        if (endDiscussionRes.IsFailure)
            return endDiscussionRes.Error;

        var saveRes = await _discussionRepository.Save(getRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        _logger.LogInformation(
            "Discussion (id {O1}) was closed",
            getRes.Value.Id);

        return getRes.Value.Id;
    }
}