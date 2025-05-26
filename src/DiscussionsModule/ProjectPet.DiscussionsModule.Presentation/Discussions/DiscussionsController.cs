using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.AddMessageToDiscussion;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.CloseDiscussion;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.DeleteMessage;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.EditMessage;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Queries.GetDiscussion;
using ProjectPet.DiscussionsModule.Contracts.Requests;
using ProjectPet.Framework;
using ProjectPet.Framework.Authorization;

namespace ProjectPet.DiscussionsModule.Presentation.Discussions;
public class DiscussionsController : CustomControllerBase
{
    [Permission(PermissionCodes.DiscussionsAdmin)]
    [HttpPut("{discussionId:guid}/close")]
    public async Task<IActionResult> DiscussionClose(
        [FromServices] CloseDiscussionHandler handler,
        [FromRoute] Guid discussionId,
        CancellationToken cancellationToken = default)
    {
        var userIdRes = GetCurrentUserId();
        if (userIdRes.IsFailure)
            return userIdRes.Error.ToResponse();

        var command = new CloseDiscussionCommand(discussionId);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.DiscussionsRead)]
    [HttpGet("{discussionId:guid}")]
    public async Task<IActionResult> DiscussionGet(
        [FromServices] GetDiscussionHandler handler,
        [FromRoute] Guid discussionId,
        CancellationToken cancellationToken = default)
    {
        var userIdRes = GetCurrentUserId();
        if (userIdRes.IsFailure)
            return userIdRes.Error.ToResponse();

        var query = new GetDiscussionQuery(discussionId);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.DiscussionsParticipate)]
    [HttpPost("{discussionId:guid}/message")]
    public async Task<IActionResult> MessageNew(
        [FromServices] AddMessageToDiscussionHandler handler,
        IValidator<AddMessageToDiscussionRequest> validator,
        [FromBody] AddMessageToDiscussionRequest request,
        [FromRoute] Guid discussionId,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var userIdRes = GetCurrentUserId();
        if (userIdRes.IsFailure)
            return userIdRes.Error.ToResponse();

        var command = AddMessageToDiscussionCommand.FromRequest(request, discussionId, userIdRes.Value);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.DiscussionsParticipate)]
    [HttpDelete("{discussionId:guid}/message/{messageId:guid}")]
    public async Task<IActionResult> MessageDelete(
        [FromServices] DeleteMessageHandler handler,
        [FromRoute] Guid discussionId,
        [FromRoute] Guid messageId,
        CancellationToken cancellationToken = default)
    {
        var userIdRes = GetCurrentUserId();
        if (userIdRes.IsFailure)
            return userIdRes.Error.ToResponse();

        var command = new DeleteMessageCommand(
            userIdRes.Value,
            discussionId,
            messageId);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.DiscussionsParticipate)]
    [HttpPatch("{discussionId:guid}/message/{messageId:guid}")]
    public async Task<IActionResult> MessageUpdate(
        [FromServices] EditMessageHandler handler,
        IValidator<EditMessageRequest> validator,
        [FromBody] EditMessageRequest request,
        [FromRoute] Guid discussionId,
        [FromRoute] Guid messageId,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var userIdRes = GetCurrentUserId();
        if (userIdRes.IsFailure)
            return userIdRes.Error.ToResponse();

        var command = EditMessageCommand.FromRequest(request, discussionId, messageId, userIdRes.Value);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
