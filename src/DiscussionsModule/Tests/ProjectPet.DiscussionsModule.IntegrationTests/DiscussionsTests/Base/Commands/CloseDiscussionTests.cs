using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Commands.CloseDiscussion;
using ProjectPet.DiscussionsModule.IntegrationTests.Factories;

namespace ProjectPet.DiscussionsModule.IntegrationTests.DiscussionsTests.Base.Commands;

public class CloseDiscussionTests : DiscussionsTestBase
{
    private CloseDiscussionHandler _sut;

    public CloseDiscussionTests(DiscussionsTestWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<CloseDiscussionHandler>();
    }

    [Fact]
    public async Task CloseDiscussion_Success()
    {
        // Arrange
        var discussion = await SeedDiscussionAsync();

        var command = new CloseDiscussionCommand(discussion.Id);

        // Act
        var result = await _sut.HandleAsync(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var discussionAssert = await _readDbContext.Discussions.FirstOrDefaultAsync();

        discussionAssert.Should().NotBeNull();
        discussionAssert.IsClosed.Should().BeTrue();
    }

    [Fact]
    public async Task CloseDiscussion_AlreadyClosed_Success()
    {
        // Arrange
        var discussion = await SeedDiscussionAsync(acts: x => x.EndDiscussion());

        // Arrange asserts
        discussion.IsClosed.Should().BeTrue();
        // 

        var command = new CloseDiscussionCommand(discussion.Id);

        // Act
        var result = await _sut.HandleAsync(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var discussionAssert = await _readDbContext.Discussions.FirstOrDefaultAsync();

        discussionAssert.Should().NotBeNull();
        discussionAssert.IsClosed.Should().BeTrue();
    }
}
