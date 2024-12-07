using CSharpFunctionalExtensions;
using System.Text.Json.Serialization;

namespace ProjectPet.AccountsModule.Domain.UserData;
public class SocialNetwork : ValueObject
{
    public string Name { get; }
    public string Url { get; }

    public SocialNetwork() { } //efcore

    [JsonConstructor]
    public SocialNetwork(string name, string url)
    {
        Name = name;
        Url = url;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Url;
    }
}
