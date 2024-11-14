﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Domain.Models;
using ProjectPet.Infrastructure.Configurations;
using CConstants = ProjectPet.Domain.Shared.Constants;

namespace ProjectPet.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FullName)
            .ConfigureString(CConstants.STRING_LEN_MEDIUM);

        builder.Property(e => e.Email)
            .ConfigureString(CConstants.STRING_LEN_MEDIUM);

        builder.Property(e => e.Description)
            .ConfigureString(CConstants.STRING_LEN_MEDIUM);

        builder.Property(e => e.YOExperience);

        builder.ComplexProperty(e => e.Phonenumber, ba =>
        {
            ba.Property(e => e.Number)
                .ConfigureString()
                .HasColumnName("phonenumber");

            ba.Property(e => e.AreaCode)
                .HasMaxLength(CConstants.STRING_LEN_SMALL)
                .HasColumnName("phonenumber_area_code");
        });

        builder.HasMany(e => e.OwnedPets)
            .WithOne()
            .HasForeignKey("pet_id")
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(e => e.PaymentMethods, d =>
        {
            d.ToJson();
            d.OwnsMany(a => a.Data, i =>
            {
                i.Property(payInfo => payInfo.Title)
                    .ConfigureString();

                i.Property(payInfo => payInfo.Instructions)
                    .ConfigureString(CConstants.STRING_LEN_MEDIUM);
            });
        });

        builder.OwnsOne(e => e.SocialNetworks, d =>
        {
            d.ToJson();
            d.OwnsMany(a => a.Data, i =>
            {
                i.Property(network => network.Name)
                    .ConfigureString();

                i.Property(network => network.Link)
                    .ConfigureString(CConstants.STRING_LEN_MEDIUM);
            });
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}
