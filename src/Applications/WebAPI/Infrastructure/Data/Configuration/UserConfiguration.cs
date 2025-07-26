using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Features.User.Models;

namespace WebAPI.Infrastructure.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("tb_user");
        builder.HasKey(e => e.id);
        builder.Property(e => e.id).HasColumnName("id").HasColumnType("uuid");

        builder.HasMany(u => u.UserProfiles)
         .WithOne(up => up.User)
         .HasForeignKey(up => up.UserId)
         .OnDelete(DeleteBehavior.Cascade);
    }
} 