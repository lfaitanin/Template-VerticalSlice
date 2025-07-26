using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Features.Profile.Models;

namespace WebAPI.Infrastructure.Data.Configuration;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("tb_profile");
        builder.HasKey(e => e.id);
        builder.Property(e => e.id).HasColumnName("id").HasColumnType("integer");
        builder.Property(e => e.description).HasColumnName("description").HasColumnType("text").IsRequired();
        builder.Property(e => e.status).HasColumnName("status").HasColumnType("text").IsRequired();

        builder.HasMany(p => p.UserProfiles)
            .WithOne(up => up.Profile)
            .HasForeignKey(up => up.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 