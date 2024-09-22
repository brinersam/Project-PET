using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .ConfigureString();

            builder.Property(e => e.Species)
                .ConfigureString();

            builder.Property(e => e.Description)
                .ConfigureString(Constants.STRING_LEN_MEDIUM);

            builder.Property(e => e.Breed)
                .ConfigureString();

            builder.Property(e => e.Coat);

            builder.Property(e => e.Health)
                .ConfigureString(Constants.STRING_LEN_MEDIUM);

            builder.Property(e => e.Address)
                .ConfigureString(Constants.STRING_LEN_MEDIUM);

            builder.Property(e => e.Weight);

            builder.Property(e => e.Height);

            builder.Property(e => e.PhoneNumber)
                .ConfigureString();

            builder.Property(e => e.IsSterilized)
                .IsRequired();

            builder.Property(e => e.IsVaccinated)
                .IsRequired();

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
                                i.Property(payInfo => payInfo.Title).ConfigureString();
                                i.Property(payInfo => payInfo.Instructions).ConfigureString(Constants.STRING_LEN_MEDIUM);
                            });
                    });

            builder.OwnsOne(e => e.Photos, d =>
            {
                d.ToJson();
                d.OwnsMany(a => a.Data, i =>
                {
                    i.Property(photo => photo.StoragePath).ConfigureString();
                    i.Property(photo => photo.IsPrimary).IsRequired();
                });
            });
        }
    }
}
