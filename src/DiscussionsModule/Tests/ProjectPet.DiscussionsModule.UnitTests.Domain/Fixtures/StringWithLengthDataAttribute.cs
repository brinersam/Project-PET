using AutoFixture;
using AutoFixture.Xunit2;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures;

public class StringWithLengthDataAttribute : AutoDataAttribute
{
    public StringWithLengthDataAttribute(int length) : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize<string>(x => x
            .FromFactory(() => string.Join(string.Empty, fixture.CreateMany<char>(length))));
        return fixture;
    })
    { }
}

public class InlineStringWithLengthDataAttribute : InlineAutoDataAttribute
{
    public InlineStringWithLengthDataAttribute(int length) : base(
        new StringWithLengthDataAttribute(length))
    { }
}