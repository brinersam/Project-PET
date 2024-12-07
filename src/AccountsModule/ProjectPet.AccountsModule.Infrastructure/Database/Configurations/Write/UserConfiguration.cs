using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.Framework.EFExtensions;

namespace ProjectPet.AccountsModule.Infrastructure.Database.Configurations.Write;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.PaymentInfos)
            .JsonVOCollectionConverter()
            .HasColumnType("jsonb")
            .HasColumnName("payment_infos")
            .IsRequired(false);

        builder.Property(u => u.SocialNetworks)
            .JsonVOCollectionConverter()
            .HasColumnName("social_networks")
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.Property(u => u.VolunteerData)
            .JsonVOConverter()
            .HasColumnType("jsonb")
            .HasColumnName("account_volunteer")
            .IsRequired(false);

        builder.Property(u => u.AdminData)
            .JsonVOConverter()
            .HasColumnType("jsonb")
            .HasColumnName("account_admin")
            .IsRequired(false);

        builder.Property(u => u.MemberData)
            .JsonVOConverter()
            .HasColumnType("jsonb")
            .HasColumnName("account_member")
            .IsRequired(false);
    }
}
