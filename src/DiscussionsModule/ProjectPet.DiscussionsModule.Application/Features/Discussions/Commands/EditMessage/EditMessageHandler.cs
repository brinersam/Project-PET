using CSharpFunctionalExtensions;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.EditMessage;
public class EditMessageHandler
{
    private readonly IDiscussionsRepository _discussionRepository;

    public EditMessageHandler(
        IDiscussionsRepository requestRepository)
    {
        _discussionRepository = requestRepository;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        EditMessageCommand command,
        CancellationToken cancellationToken)
    {
        var getRes = await _discussionRepository.GetByIdAsync(command.DiscussionId, cancellationToken);
        if (getRes.IsFailure)
            return getRes.Error;

        var editCommentRes = getRes.Value.EditComment(
            command.UserId,
            command.MessageId,
            command.MessageBody);
        if (editCommentRes.IsFailure)
            return editCommentRes.Error;

        var saveRes = await _discussionRepository.Save(getRes.Value, cancellationToken);
        if (saveRes.IsFailure)
            return saveRes.Error;

        return getRes.Value.Id;
    }
}