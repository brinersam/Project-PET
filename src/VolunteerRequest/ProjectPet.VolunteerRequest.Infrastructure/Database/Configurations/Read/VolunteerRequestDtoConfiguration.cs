using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Framework.EFExtensions;
using ProjectPet.VolunteerRequests.Contracts.Dto;
using ProjectPet.VolunteerRequests.Domain.Models;

namespace ProjectPet.VolunteerRequests.Infrastructure.Database.Configurations.Read;
internal class VolunteerRequestDtoConfiguration : IEntityTypeConfiguration<VolunteerRequestDto>
{
    public void Configure(EntityTypeBuilder<VolunteerRequestDto> builder)
    {
        builder.ToTable($"{nameof(VolunteerRequest)}s");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.VolunteerData)
            .JsonVOConverter()
            .HasColumnName("volunteer_account_data")
            .IsRequired(false)
            .HasColumnType("jsonb");

        builder.Property(x => x.Status)
            .HasConversion<string>();
    }
}
