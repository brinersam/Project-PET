using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;
using System.Text.Json.Serialization;

namespace ProjectPet.Domain.Models;

public record SocialNetwork
{
    public string Name { get; } = null!;
    public string Link { get; } = null!;

    [JsonConstructor]
    private SocialNetwork(string link, string name)
    {
        Name = name;
        Link = link;
    }

    public static Result<SocialNetwork, Error> Create(string link, string name)
    {
        var validator = Validator.ValidatorString();

        var result = validator.Check(name, nameof(name));
        if (result.IsFailure)
            return result.Error;

        result = validator
            .SetMaxLen(Constants.STRING_LEN_MEDIUM)
            .Check(link, nameof(link));

        if (result.IsFailure)
            return result.Error;

        return new SocialNetwork(link, name);
    }
}
