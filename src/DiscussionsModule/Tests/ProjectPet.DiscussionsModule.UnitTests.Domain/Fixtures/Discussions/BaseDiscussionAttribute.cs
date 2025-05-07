using AutoFixture;
using AutoFixture.Xunit2;
using ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions.Customizations;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions;
public class BaseDiscussionAttribute : AutoDataAttribute
{
    public BaseDiscussionAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new BaseDiscussionCustomization());

        return fixture;
    })
    { }
}

