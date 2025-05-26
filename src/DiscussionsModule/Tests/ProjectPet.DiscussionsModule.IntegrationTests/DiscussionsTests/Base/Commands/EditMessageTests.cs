using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.EditMessage;
using ProjectPet.DiscussionsModule.IntegrationTests.Factories;

namespace ProjectPet.DiscussionsModule.IntegrationTests.DiscussionsTests.Base.Commands;

public class EditMessageTests : DiscussionsTestBase
{
    private EditMessageHandler _sut;

    public EditMessageTests(DiscussionsTestWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<EditMessageHandler>();
    }

    [Fact]
    public async Task AddMessageToDiscussion_Success()
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

        var command = new EditMessageCommand(
            userIds[0],
            discussion.Id,
            message.Id,
            newMessageText);

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
        messageAssert.Should().NotBeNull();

        // message was updated
        messageAssert.Text.Should().NotBeEquivalentTo(initialMessageText);
        messageAssert.Text.Should().BeEquivalentTo(newMessageText);

        // everything else remains the same
        messageAssert.Id.Should().Be(message.Id);
        messageAssert.UserId.Should().Be(message.UserId);
    }
}