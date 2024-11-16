using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.Application.Dto;

namespace ProjectPet.Infrastructure.Configurations.Read;
public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerReadDto>
{
    public void Configure(EntityTypeBuilder<VolunteerReadDto> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(x => x.Id);
    }
}
