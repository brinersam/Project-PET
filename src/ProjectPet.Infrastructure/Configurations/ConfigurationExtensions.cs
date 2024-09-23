using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Infrastructure.Configurations
{
    internal static class ConfigurationExtensions
    {
        public static PropertyBuilder<T> ConfigureString<T>(this PropertyBuilder<T> prop, int maxLen = Constants.STRING_LEN_SMALL)
        {
            return prop
                .IsRequired()
                .HasMaxLength(maxLen);
        }
    }
}
