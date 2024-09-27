using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;
using System.Text.RegularExpressions;

namespace ProjectPet.Domain.Models
{
    public record PhoneNumber
    {
        private const string REGEX = "^\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{2}[\\s.-]?\\d{2}$";

        public string AreaCode { get; } = null!;
        public string Number { get; } = null!;

        private PhoneNumber(string number, string areaCode)
        {
            Number = number;
            AreaCode = areaCode;
        }

        public static Result<PhoneNumber, Error> Create(string number, string areaCode)
        {
            var result = Validator
                .ValidatorString(maxLen : 4)
                .Check(areaCode, nameof(areaCode));

            if (result.IsFailure)
                return result.Error;

            if (!Regex.IsMatch(number, REGEX))
                return Error.Validation("value.is.invalid", "Phone number must be 10 characters long, with optional - inbetween number groups");

            return new PhoneNumber(number, areaCode);
        }
    }
}