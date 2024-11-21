using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Domain.Models;

public record HealthInfo
{
    public string Health { get; } = null!;
    public bool IsSterilized { get; }
    public bool IsVaccinated { get; }
    public float Weight { get; }
    public float Height { get; }

    private HealthInfo(
        string health,
        bool isSterilized,
        bool isVaccinated,
        float weight,
        float height)
    {
        Health = health;
        IsSterilized = isSterilized;
        IsVaccinated = isVaccinated;
        Weight = weight;
        Height = height;
    }

    public static Result<HealthInfo, Error> Create(
        string health,
        bool isSterilized,
        bool isVaccinated,
        float weight,
        float height)
    {
        var result = Validator.ValidatorString(Constants.STRING_LEN_MEDIUM)
            .Check(health, nameof(health));

        if (result.IsFailure)
            return result.Error;

        return new HealthInfo
        (
            health,
            isSterilized,
            isVaccinated,
            weight,
            height
        );
    }
}