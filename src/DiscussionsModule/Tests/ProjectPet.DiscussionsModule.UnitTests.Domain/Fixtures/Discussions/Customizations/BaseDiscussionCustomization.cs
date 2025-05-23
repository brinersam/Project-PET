using AutoFixture;
using ProjectPet.DiscussionsModule.Domain.Models;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions.Customizations;
public class BaseDiscussionCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.RepeatCount = 2;

        Discussion discussion = Discussion.Create(
                fixture.Create<Guid>(),
                fixture.CreateMany<Guid>()).Value;

        fixture.Freeze<Discussion>(); // freeze() it so we can alter this thing using more customizations
                                      // wont work without freeze() since using Do() wont work with factories lol

        fixture.Customize<Discussion>(opts => opts
            .FromFactory(() => discussion));
    }
}
