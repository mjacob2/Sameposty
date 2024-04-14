using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sameposty.DataAccess.DatabaseContext;
public class SamepostyDbContextFactory : IDesignTimeDbContextFactory<SamepostyDbContext>
{
    public SamepostyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SamepostyDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SamepostyDb;Trusted_Connection=True");
        // optionsBuilder.UseSqlServer("Server=tcp:sameposty.database.windows.net,1433;Initial Catalog=SamepostyDb;Persist Security Info=False;User ID=marek;Password=4eNn*!rGkmB&%t;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
        return new SamepostyDbContext(optionsBuilder.Options);
    }
}


// Server=tcp:sameposty.database.windows.net,1433;Initial Catalog=SamepostyDb;Persist Security Info=False;User ID=marek;Password=4eNn*!rGkmB&%t;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

//Server=(localdb)\\mssqllocaldb;Database=SamepostyDb;Trusted_Connection=True;