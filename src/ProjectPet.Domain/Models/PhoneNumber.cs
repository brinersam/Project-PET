using System.Text.RegularExpressions;

namespace ProjectPet.Domain.Models
{
    public record PhoneNumber
    {
        private const string REGEX = "^\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{2}[\\s.-]?\\d{2}$";

        public string? AreaCode { get; }
        public string Number { get; } = null!;


        protected PhoneNumber(string number, string? areaCode)
        {
            Number = number;
            AreaCode = areaCode;
        }

        public static PhoneNumber Create(string number, string? areaCode) // TODO change to result
        {
            if (!Regex.IsMatch(number, REGEX))
                throw new ArgumentException("Phone number must be 10 characters long, with optional - inbetween number groups");

            return new PhoneNumber(number,areaCode);
        }
    }
}