using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.AccountsModule.Domain;

namespace ProjectPet.AccountsModule.Infrastructure.Database.Configurations.Write;
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasIndex(p => p.Code)
            .IsUnique(true);
    }
}
