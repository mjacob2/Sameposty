using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.DatabaseContext;
public class SamepostyDbContext(DbContextOptions<SamepostyDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<SocialMediaConnection> SocialMediaConnections { get; set; }

    public DbSet<BasicInformation> BasicInformations { get; set; }
}
