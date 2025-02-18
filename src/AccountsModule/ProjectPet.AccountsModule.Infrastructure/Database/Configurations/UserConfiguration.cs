﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.Framework.EFExtensions;

namespace ProjectPet.AccountsModule.Infrastructure.Database.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        builder.Property(u => u.SocialNetworks)
            .JsonVOCollectionConverter()
            .HasColumnName("social_networks")
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.Property(x => x.VolunteerData)
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

