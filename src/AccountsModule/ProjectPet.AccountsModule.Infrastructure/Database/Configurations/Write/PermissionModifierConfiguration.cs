using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.AccountsModule.Domain;

namespace ProjectPet.AccountsModule.Infrastructure.Database.Configurations.Write;
internal class PermissionModifierConfiguration : IEntityTypeConfiguration<PermissionModifier>
{
    public void Configure(EntityTypeBuilder<PermissionModifier> builder)
    {
        builder.ToTable("permission_modifiers");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.ExpiresAt)
            .HasDefaultValue(DateTime.UtcNow.AddDays(1));
    }
}
