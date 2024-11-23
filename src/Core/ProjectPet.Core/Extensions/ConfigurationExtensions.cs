using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CConstants = ProjectPet.SharedKernel.Constants;

namespace ProjectPet.Core.Extensions;

public static class ConfigurationExtensions
{

    public static PropertyBuilder<T> ConfigureString<T>(this PropertyBuilder<T> prop, int maxLen = CConstants.STRING_LEN_SMALL)
    {
        return prop
            .IsRequired()
            .HasMaxLength(maxLen);
    }

    // no dry : cant access IInfrastructure<IConventionPropertyBuilder> :(
    public static ComplexTypePropertyBuilder<T> ConfigureString<T>(this ComplexTypePropertyBuilder<T> prop, int maxLen = CConstants.STRING_LEN_SMALL)
    {
        return prop
            .IsRequired()
            .HasMaxLength(maxLen);
    }

}
