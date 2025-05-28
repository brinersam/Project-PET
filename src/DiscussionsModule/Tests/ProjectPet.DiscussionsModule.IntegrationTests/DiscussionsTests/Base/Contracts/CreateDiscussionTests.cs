using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.DiscussionsModule.Contracts;
using ProjectPet.DiscussionsModule.IntegrationTests.Factories;

namespace ProjectPet.DiscussionsModule.IntegrationTests.DiscussionsTests.Base.Contracts;

public class CreateDiscussionTests : DiscussionsTestBase
{
    private IDiscussionModuleContract _sut;

    public CreateDiscussionTests(DiscussionsTestWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<IDiscussionModuleContract>();
    }

    [Fact]
    public async Task CreateDiscussion_Success()
    {
        // Arrange
        List<Guid> userIds = [Guid.NewGuid(), Guid.NewGuid()];
        Guid entityId = Guid.NewGuid();

        // Act
        var result = await _sut.CreateDiscussionAsync(entityId, userIds);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var discussion = await _readDbContext.Discussions.FirstOrDefaultAsync();
        discussion.Should().NotBeNull();
        discussion.UserIds.Should().NotBeEmpty();
        discussion.UserIds.Should().Contain(userIds);
    }

    [Fact]
    public async Task CreateDiscussion_DefaultEntityId_Failure()
    {
        // Arrange
        List<Guid> userIds = [Guid.NewGuid(), Guid.NewGuid()];
        Guid entityId = default;

        // Act
        var result = await _sut.CreateDiscussionAsync(entityId, userIds);

        // Assert
        result.IsSuccess.Should().BeFalse();
        var discussion = await _readDbContext.Discussions.FirstOrDefaultAsync();
        discussion.Should().BeNull();
    }
}
