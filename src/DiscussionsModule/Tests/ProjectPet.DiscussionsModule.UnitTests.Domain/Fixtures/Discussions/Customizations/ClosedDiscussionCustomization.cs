using AutoFixture;
using ProjectPet.DiscussionsModule.Domain.Models;
namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions.Customizations;

public class ClosedDiscussionCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var discussion = fixture.Freeze<Discussion>();

        discussion.EndDiscussion();
    }
}
