using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ValueObjects;

namespace ProjectPet.VolunteerRequests.Domain.Models;
public class VolunteerAccountData : ValueObject
{
    public List<PaymentInfo> PaymentInfos { get; init; }
    public int Experience { get; init; }
    public string[] Certifications { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public VolunteerAccountData() : base()
    { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public VolunteerAccountData(IEnumerable<PaymentInfo> paymentInfos, int experience, string[] certifications)
    {
        PaymentInfos = paymentInfos.ToList();
        Experience = experience;
        Certifications = certifications;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Experience;
        yield return Certifications.GetHashCode();
        yield return PaymentInfos.GetHashCode();
    }
}