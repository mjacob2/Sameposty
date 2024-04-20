using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Executors;

[assembly: FunctionsStartup(typeof(SubscriptionCreatedFunction.Startup))]
namespace SubscriptionCreatedFunction;
internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddDbContext<SamepostyDbContext>(options =>
            options.UseSqlServer("Server=SURFACEPRO7-MAR\\SQLEXPRESS;Initial Catalog=SamepostyDb3;User ID=sa;Password=KOnefka$3101;Trust Server Certificate=True;"));
        builder.Services.AddSingleton<IQueryExecutor, QueryExecutor>();
    }
}
