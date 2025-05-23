using AutoFixture;
using AutoFixture.Xunit2;
using ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions.Customizations;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions;
public class CommentsDiscussionAttribute : AutoDataAttribute
{
    public CommentsDiscussionAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new CompositeCustomization(
            new BaseDiscussionCustomization(),
            new GenerateCommentsCustomization()
        ));

        return fixture;
    })
    { }
}
