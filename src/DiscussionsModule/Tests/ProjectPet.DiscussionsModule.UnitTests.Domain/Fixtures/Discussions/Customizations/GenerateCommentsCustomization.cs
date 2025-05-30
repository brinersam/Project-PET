using AutoFixture;
using ProjectPet.DiscussionsModule.Domain.Models;
using System.Reflection;

namespace ProjectPet.DiscussionsModule.UnitTests.Domain.Fixtures.Discussions.Customizations;
public class GenerateCommentsCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var discussion = fixture.Freeze<Discussion>();

        GenerateComments(fixture, discussion);
        AddIdToComments(fixture, discussion);
    }

    private static void AddIdToComments(IFixture fixture, Discussion discussion)
    {
        foreach (var comment in discussion.Messages)
        {
            Type type = typeof(Message);
            PropertyInfo idProperty = type.BaseType?.GetProperty(nameof(Message.Id), BindingFlags.Instance | BindingFlags.Public)!;

            var setMethod = idProperty.GetSetMethod(true);
            setMethod.Invoke(comment, new object[] { fixture.Create<Guid>() });
        }
    }

    private static void GenerateComments(IFixture fixture, Discussion discussion)
    {
        foreach (Guid userId in discussion.UserIds)
            discussion.AddComment(userId, fixture.Create<string>());
    }
}

