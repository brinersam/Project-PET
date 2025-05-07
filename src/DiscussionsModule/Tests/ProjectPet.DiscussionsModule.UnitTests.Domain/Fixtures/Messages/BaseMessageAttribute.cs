using AutoFixture;
using AutoFixture.Xunit2;
using ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Messages.Customizations;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Messages;
public class BaseMessageAttribute : AutoDataAttribute
{
    public BaseMessageAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new BaseMessageCustomization());

        return fixture;
    })
    { }
}
