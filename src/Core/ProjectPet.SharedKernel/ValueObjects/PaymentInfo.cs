using CSharpFunctionalExtensions;
using System.Text.Json.Serialization;

namespace ProjectPet.SharedKernel.ValueObjects;
public class PaymentInfo : ValueObject
{
    public string Title { get; }
    public string Instructions { get; }

    public PaymentInfo() { } //efcore

    [JsonConstructor]
    public PaymentInfo(string title, string instructions)
    {
        Title = title;
        Instructions = instructions;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Instructions;
    }
}
