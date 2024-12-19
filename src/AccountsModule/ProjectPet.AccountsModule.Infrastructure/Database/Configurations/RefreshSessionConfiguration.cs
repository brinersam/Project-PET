using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectPet.AccountsModule.Domain;

namespace ProjectPet.AccountsModule.Infrastructure.Database.Configurations;
public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder.ToTable("refresh_sessions");

        builder.HasKey(rs => rs.Id);

        builder.HasOne<User>(rs => rs.User)
            .WithMany()
            .HasForeignKey(rs => rs.UserId);
    }
}
