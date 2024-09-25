using System.Text.Json.Serialization;

namespace ProjectPet.Domain.Models
{
    public record PaymentInfo
    {
        public string Title { get; } = null!;
        public string Instructions { get; } = null!;

        [JsonConstructor]
        protected PaymentInfo(string title, string instructions)
        {
            Title = title;
            Instructions = instructions;
        }

        public static PaymentInfo Create(string title, string instructions) // TODO change to result
        {
            if (String.IsNullOrWhiteSpace(title)) 
                throw new ArgumentNullException("Title should not be empty"); // TODO return result

            if (String.IsNullOrWhiteSpace(instructions))
                throw new ArgumentNullException("instructions should not be empty"); // TODO return result

            return new PaymentInfo(title, instructions);
        }
    }
}

