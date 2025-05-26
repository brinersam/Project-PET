using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.DiscussionsModule.Application.Features.Discussions.Queries.GetDiscussion;
using ProjectPet.DiscussionsModule.IntegrationTests.Factories;

namespace ProjectPet.DiscussionsModule.IntegrationTests.DiscussionsTests.Base.Commands;

public class GetDiscussionTests : DiscussionsTestBase
{
    private GetDiscussionHandler _sut;

    public GetDiscussionTests(DiscussionsTestWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<GetDiscussionHandler>();
    }

    [Fact]
    public async Task GetDiscussion_Success()
    {
        // Arrange
        Guid[] userIds = [Guid.NewGuid(), Guid.NewGuid()];
        var senderUserId = userIds[0];
        var message1Text = _fixture.Create<string>();
        var message2Text = _fixture.Create<string>();

        var discussion = await SeedDiscussionAsync(
            userIds: userIds,
            acts: x => 
        {
            x.AddComment(senderUserId, message1Text);
            x.AddComment(senderUserId, message2Text);
        });

        var query = new GetDiscussionQuery(discussion.Id);

        // Act
        var result = await _sut.HandleAsync(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var discussionAssert = await _readDbContext
            .Discussions
            .Include(x => x.Messages)
            .FirstOrDefaultAsync();
        discussionAssert.Should().NotBeNull();

        discussionAssert.Messages.Should().Contain(x => x.Text == message1Text);
        discussionAssert.Messages.Should().Contain(x => x.Text == message2Text);
    }
}