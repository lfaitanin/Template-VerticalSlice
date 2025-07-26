using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Features.User.Models;

namespace WebAPI.Infrastructure.Data.Configuration;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("tb_user_profile");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid");

        builder.HasOne(up => up.User)
            .WithMany(u => u.UserProfiles)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(up => up.Profile)
            .WithMany(p => p.UserProfiles)
            .HasForeignKey(up => up.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 