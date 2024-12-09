using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Domain.UserData;
using System.Text.Json.Serialization;

namespace ProjectPet.AccountsModule.Domain.Accounts;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class VolunteerAccount : ValueObject
{
    public const string ROLENAME = "Volunteer";
    [JsonIgnore]
    public IReadOnlyList<PaymentInfo> PaymentInfos => _paymentInfos;
    [JsonInclude]
    [JsonPropertyName("PaymentInfos")]
    private List<PaymentInfo> _paymentInfos;
    public int Experience { get; init; }
    public string[] Certifications { get; init; } //todo vo

    public VolunteerAccount(IEnumerable<PaymentInfo> _paymentInfos, int experience, string[] certifications) :
        this(_paymentInfos.ToList(), experience, certifications) { }
    
    [JsonConstructor]
    public VolunteerAccount(List<PaymentInfo> _paymentInfos, int experience, string[] certifications)
    {
        this._paymentInfos = _paymentInfos;
        Experience = experience;
        Certifications = certifications;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Experience;
        yield return Certifications.GetHashCode();
        yield return _paymentInfos.GetHashCode();
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.