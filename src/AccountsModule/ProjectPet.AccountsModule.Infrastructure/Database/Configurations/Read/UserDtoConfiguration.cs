using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.Core.Extensions;

namespace ProjectPet.AccountsModule.Infrastructure.Database.Configurations.Read;
public class UserDtoConfiguration : IEntityTypeConfiguration<UserDto>
{
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        //builder.Property(x => x.Roles);

        builder.Property(x => x.SocialNetworks)
            .JsonVOCollectionConverter()
            .IsRequired(false);

        builder.Property(x => x.VolunteerData)
            .JsonVOConverter()
            .HasColumnName("account_volunteer")
            .IsRequired(false);

        builder.Property(x => x.AdminData)
            .JsonVOConverter()
            .HasColumnName("account_admin")
            .IsRequired(false);

        builder.Property(x => x.MemberData)
            .JsonVOConverter()
            .HasColumnName("account_member")
            .IsRequired(false);
    }
}
