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

            builder.Property(e => e.PhoneNumber)
                .ConfigureString();

            builder.HasMany(e => e.OwnedPets)
                .WithOne()
                .HasForeignKey("pet_id")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
