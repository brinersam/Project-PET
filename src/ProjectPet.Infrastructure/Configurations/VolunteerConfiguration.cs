using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Infrastructure.Configurations
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.FullName)
                .ConfigureString(Constants.STRING_LEN_MEDIUM);

            builder.Property(e => e.Email)
                .ConfigureString(Constants.STRING_LEN_MEDIUM);

            builder.Property(e => e.Description)
                .ConfigureString(Constants.STRING_LEN_MEDIUM);

            builder.Property(e => e.Email)
                .ConfigureString(Constants.STRING_LEN_MEDIUM);

            builder.Property(e => e.YOExperience);

            builder.ComplexProperty(e => e.PhoneNumber, ba =>
            {
                ba.Property(e => e.Number)
                    .ConfigureString()
                    .HasColumnName("number");

                ba.Property(e => e.AreaCode)
                    .HasMaxLength(Constants.STRING_LEN_SMALL)
                    .HasColumnName("area_code");
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
                        .ConfigureString(Constants.STRING_LEN_MEDIUM);
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
                        .ConfigureString(Constants.STRING_LEN_MEDIUM);
                });
            });
        }
    }
}
