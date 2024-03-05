using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sameposty.DataAccess.DatabaseContext;

namespace Sameposty.API;
public class SamepostyDbContextFactory : IDesignTimeDbContextFactory<SamepostyDbContext>
{
    public SamepostyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SamepostyDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SamepostyDb;Trusted_Connection=True;");
        return new SamepostyDbContext(optionsBuilder.Options);
    }
}
