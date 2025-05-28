using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.DeleteMessage;
using ProjectPet.DiscussionsModule.IntegrationTests.Factories;

namespace ProjectPet.DiscussionsModule.IntegrationTests.DiscussionsTests.Base.Commands;

public class DeleteMessageTests : DiscussionsTestBase
{
    private DeleteMessageHandler _sut;

    public DeleteMessageTests(DiscussionsTestWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<DeleteMessageHandler>();
    }

    [Fact]
    public async Task DeleteMessageFromDiscussion_Success()
    {
        // Arrange
        Guid[] userIds = [Guid.NewGuid(), Guid.NewGuid()];
        var senderUserId = userIds[0];
        var initialMessageText = _fixture.Create<string>();
        var newMessageText = _fixture.Create<string>();

        var discussion = await SeedDiscussionAsync(
            userIds: userIds,
            acts: x => x.AddComment(senderUserId, initialMessageText));

        var message = discussion.Messages.FirstOrDefault();

        var command = new DeleteMessageCommand(
            userIds[0],
            discussion.Id,
            message.Id);

        // Act
        var result = await _sut.HandleAsync(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var discussionAssert = await _readDbContext
            .Discussions
            .Include(x => x.Messages)
            .FirstOrDefaultAsync();
        discussionAssert.Should().NotBeNull();

        var messageAssert = discussionAssert.Messages.FirstOrDefault();
    }

    [Fact]
    public async Task DeleteMessageFromDiscussion_MessageDoesNotExist_Success()
    {
        // Arrange
        Guid[] userIds = [Guid.NewGuid(), Guid.NewGuid()];
        var senderUserId = userIds[0];

        var discussion = await SeedDiscussionAsync(userIds: userIds);

        var command = new DeleteMessageCommand(
            userIds[0],
            discussion.Id,
            _fixture.Create<Guid>());

        // Act
        var result = await _sut.HandleAsync(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}