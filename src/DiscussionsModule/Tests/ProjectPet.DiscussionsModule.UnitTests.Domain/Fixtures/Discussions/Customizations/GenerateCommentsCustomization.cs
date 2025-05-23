using AutoFixture;
using ProjectPet.DiscussionsModule.Domain.Models;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions.Customizations;
public class GenerateCommentsCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var discussion = fixture.Freeze<Discussion>();

        GenerateComments(fixture, discussion);
    }

    private static void GenerateComments(IFixture fixture, Discussion discussion)
    {
        foreach (Guid userId in discussion.UserIds)
            discussion.AddComment(userId, fixture.Create<string>());
    }
}

