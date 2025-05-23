using AutoFixture;
using AutoFixture.Xunit2;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures;
public class AmntOfGuidsAttribute : AutoDataAttribute
{
    public AmntOfGuidsAttribute(int amnt) : base(() =>
    {
        var fixture = new Fixture();
        var list = new List<Guid>();

        fixture.RepeatCount = amnt;
        fixture.AddManyTo(list);

        fixture.Customize<List<Guid>>(x => x
            .FromFactory(() => list));

        return fixture;
    })
    { }
}

public class InlineAmntOfGuidsAttribute : InlineAutoDataAttribute
{
    public InlineAmntOfGuidsAttribute(int num) : base(
        new AmntOfGuidsAttribute(num))
    { }
}
