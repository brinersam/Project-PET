using System.Text.Json.Serialization;

namespace ProjectPet.Domain.Models
{
    public record SocialNetwork
    {
        public string? Name { get; }
        public string Link { get; } = null!;

        [JsonConstructor]
        protected SocialNetwork(string link, string? name)
        {
            Name = name;
            Link = link;
        }

        public static SocialNetwork Create(string link, string? name) // TODO return result
        {
            if (String.IsNullOrWhiteSpace(link))
                throw new ArgumentNullException("Link argument should not be empty"); 

            return new SocialNetwork(link, name);
        }
    }
}
