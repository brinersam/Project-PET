using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;
using System.Text.Json.Serialization;

namespace ProjectPet.Domain.Models
{
    public record PaymentInfo
    {
        public string Title { get; } = null!;
        public string Instructions { get; } = null!;

        [JsonConstructor]
        private PaymentInfo(string title, string instructions)
        {
            Title = title;
            Instructions = instructions;
        }

        public static Result<PaymentInfo,Error> Create(string title, string instructions)
        {
            var validator = Validator.ValidatorString();

            var result = validator
                .Check(title, nameof(title));

            if (result.IsFailure)
                return result.Error;

            result = validator
                .SetMaxLen(Constants.STRING_LEN_MEDIUM)
                .Check(instructions, nameof(instructions));

            if (result.IsFailure)
                return result.Error;

            return new PaymentInfo(title, instructions);
        }
    }
}

