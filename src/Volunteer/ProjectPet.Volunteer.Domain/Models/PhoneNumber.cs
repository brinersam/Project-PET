using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;
using System.Text.RegularExpressions;

namespace ProjectPet.VolunteerModule.Domain.Models;

public record Phonenumber
{
    private const string REGEX = "^\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{2}[\\s.-]?\\d{2}$";

    public string AreaCode { get; } = null!;
    public string Number { get; } = null!;

    private Phonenumber(string number, string areaCode)
    {
        Number = number;
        AreaCode = areaCode;
    }

    public static Result<Phonenumber, Error> Create(string number, string areaCode)
    {
        var result = Validator
            .ValidatorString(maxLen: 4)
            .Check(areaCode, nameof(areaCode));

        if (result.IsFailure)
            return result.Error;

        if (!Regex.IsMatch(number, REGEX))
            return Error.Validation("value.is.invalid", "Phone number must adhere to format : 123-456-78-90 or 1234567890");

        return new Phonenumber(number, areaCode);
    }
}