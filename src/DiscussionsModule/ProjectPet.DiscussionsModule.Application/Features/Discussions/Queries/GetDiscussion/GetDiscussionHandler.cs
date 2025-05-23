using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.CloseDiscussion;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.DiscussionsModule.Contracts.Dto;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Application.Features.Discussions.Queries.GetDiscussion;
public class GetDiscussionHandler
{
    private readonly IReadDbContext _dbContext;

    public GetDiscussionHandler(
        IReadDbContext dbContext,
        ILogger<GetDiscussionHandler> logger)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DiscussionDto, Error>> HandleAsync(
        GetDiscussionQuery command,
        CancellationToken cancellationToken)
    {
        var discussion = await _dbContext.Discussions.FirstOrDefaultAsync(x => x.Id == command.DiscussionId, cancellationToken);
        if (discussion is null)
            return Errors.General.NotFound(typeof(Discussion), command.DiscussionId);

        return discussion;
    }
}