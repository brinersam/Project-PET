using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.AddMessageToDiscussion;
using ProjectPet.DiscussionsModule.IntegrationTests.Factories;

namespace ProjectPet.DiscussionsModule.IntegrationTests.DiscussionsTests.Base.Commands;

public class AddMessageToDiscussionTests : DiscussionsTestBase
{
    private AddMessageToDiscussionHandler _sut;

    public AddMessageToDiscussionTests(DiscussionsTestWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<AddMessageToDiscussionHandler>();
    }

    [Fact]
    public async Task AddMessageToDiscussion_Success()
    {
        // Arrange
        Guid[] userIds = [Guid.NewGuid(), Guid.NewGuid()];
        var discussion = await SeedDiscussionAsync(userIds: userIds);
        var message = _fixture.Create<string>();

        var command = new AddMessageToDiscussionCommand(discussion.Id, userIds[0], message);

        // Act
        var result = await _sut.HandleAsync(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var discussionAssert = await _readDbContext
            .Discussions
            .Include(x => x.Messages)
            .FirstOrDefaultAsync();

        discussionAssert.Should().NotBeNull();
        discussionAssert.Messages.Should().Contain(x => x.Text == message);
    }
}