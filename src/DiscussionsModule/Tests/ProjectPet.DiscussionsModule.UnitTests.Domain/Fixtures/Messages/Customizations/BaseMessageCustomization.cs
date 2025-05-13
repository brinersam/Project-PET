using AutoFixture;
using ProjectPet.DiscussionsModule.Domain.Models;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Messages.Customizations;
public class BaseMessageCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        Message message = Message.Create(
                fixture.Create<Guid>(),
                fixture.Create<string>()).Value;

        fixture.Freeze<Message>();

        fixture.Customize<Message>(opts => opts
            .FromFactory(() => message));
    }
}