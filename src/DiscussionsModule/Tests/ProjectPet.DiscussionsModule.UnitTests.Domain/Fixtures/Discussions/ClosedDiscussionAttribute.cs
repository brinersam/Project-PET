using AutoFixture.Xunit2;
using AutoFixture;
using ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions.Customizations;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions;

public class ClosedDiscussionAttribute : AutoDataAttribute
{
    public ClosedDiscussionAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new CompositeCustomization(
            new BaseDiscussionCustomization(),
            new GenerateCommentsCustomization(),
            new ClosedDiscussionCustomization()
        ));

        return fixture;
    })
    { }
}