using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Profile.Models;
using WebAPI.Features.PreRegistration.Models;
using WebAPI.Features.User.Models;

namespace WebAPI.Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<UserPreRegistration> UserPreRegistrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table configurations
        modelBuilder.Entity<Profile>().ToTable("tb_profile");
        modelBuilder.Entity<User>().ToTable("tb_user");
        modelBuilder.Entity<UserProfile>().ToTable("tb_user_profile");
        modelBuilder.Entity<UserPreRegistration>().ToTable("tb_user_pre_registration");
        
        // Apply all entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

