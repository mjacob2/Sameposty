using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.DatabaseContext;
public class SamepostyDbContext(DbContextOptions<SamepostyDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<SocialMediaConnection> SocialMediaConnections { get; set; }

    public DbSet<BasicInformation> BasicInformations { get; set; }

    public DbSet<Privilege> Privileges { get; set; }

    public DbSet<PublishResult> PublishResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SocialMediaConnection>()
            .Property(p => p.Platform)
            .HasConversion(
                v => v.ToString(),
                v => (SocialMediaPlatform)Enum.Parse(typeof(SocialMediaPlatform), v)
            );

        modelBuilder.Entity<User>()
            .Property(p => p.Role)
            .HasConversion(
                v => v.ToString(),
                v => (Roles)Enum.Parse(typeof(Roles), v)
            );

        modelBuilder.Entity<PublishResult>()
            .Property(p => p.Platform)
            .HasConversion(
                v => v.ToString(),
                v => (SocialMediaPlatform)Enum.Parse(typeof(SocialMediaPlatform), v)
            );

        base.OnModelCreating(modelBuilder);
    }
}
