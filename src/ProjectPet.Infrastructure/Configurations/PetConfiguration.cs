using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .ConfigureString();

        builder.ComplexProperty(e => e.AnimalData, ba =>
        {
            ba.Property(x => x.SpeciesID)
                .HasConversion(
                    push => push.Value,
                    pull => SpeciesID.New(pull));

            ba.Property(e => e.BreedID).IsRequired();
        });

        builder.Property(e => e.Description)
            .ConfigureString(Constants.STRING_LEN_MEDIUM);

        builder.Property(e => e.Coat);

        builder.ComplexProperty(e => e.HealthInfo, ba =>
        {
            ba.Property(e => e.Health)
                .ConfigureString(Constants.STRING_LEN_MEDIUM)
                .HasColumnName("health_info");

            ba.Property(e => e.IsSterilized)
                .IsRequired()
                .HasColumnName("is_sterilized");

            ba.Property(e => e.IsVaccinated)
                .IsRequired()
                .HasColumnName("is_vaccinated");

            ba.Property(e => e.Weight)
                .HasColumnName("weight");

            ba.Property(e => e.Height)
                .HasColumnName("height");
        });

        builder.ComplexProperty(e => e.Address, ba =>
        {
            ba.Property(e => e.Name)
                .ConfigureString()
                .HasColumnName("saved_name");

            ba.Property(e => e.Street)
                .IsRequired()
                .HasColumnName("street");

            ba.Property(e => e.Building)
                .IsRequired()
                .HasColumnName("building");

            ba.Property(e => e.Block).
                HasColumnName("block");

            ba.Property(e => e.Entrance)
                .HasColumnName("entrance");

            ba.Property(e => e.Floor)
                .HasColumnName("floor");

            ba.Property(e => e.Apartment)
                .IsRequired()
                .HasColumnName("apartment");
        });

        builder.ComplexProperty(e => e.PhoneNumber, ba =>
        {
            ba.Property(e => e.Number)
                .ConfigureString();

            ba.Property(e => e.AreaCode)
                .ConfigureString();

        });


        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion(
                status => status.ToString(),
                status => Enum.Parse<Status>(status)
            );

        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        builder.Property(e => e.CreatedOn)
            .IsRequired();

        builder.OwnsOne(e => e.PaymentMethods, d =>
                {
                    d.ToJson();
                    d.OwnsMany(a => a.Data, i =>
                        {
                            i.Property(payInfo => payInfo.Title)
                                .ConfigureString();

                            i.Property(payInfo => payInfo.Instructions)
                                .ConfigureString(Constants.STRING_LEN_MEDIUM);
                        });
                });

        builder.OwnsOne(e => e.Photos, d =>
        {
            d.ToJson();
            d.OwnsMany(a => a.Data, i =>
            {
                i.Property(photo => photo.StoragePath)
                    .ConfigureString();

                i.Property(photo => photo.IsPrimary)
                    .IsRequired();
            });
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}
