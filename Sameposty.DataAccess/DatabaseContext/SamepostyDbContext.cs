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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SocialMediaConnection>()
            .Property(p => p.Platform)
            .HasConversion(
                v => v.ToString(),
                v => (SocialMediaConnection.SocialMediaPlatform)Enum.Parse(typeof(SocialMediaConnection.SocialMediaPlatform), v)
            );

        modelBuilder.Entity<User>()
            .Property(p => p.Role)
            .HasConversion(
                v => v.ToString(),
                v => (Roles)Enum.Parse(typeof(Roles), v)
            );

        base.OnModelCreating(modelBuilder);
    }
}
