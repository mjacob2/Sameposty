using Microsoft.Extensions.DependencyInjection;
using Sameposty.DataAccess.Executors;

namespace Sameposty.DataAccess;
public static class DependencyInjection
{
    public static void AddSamepostyDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IQueryExecutor, QueryExecutor>();
        services.AddScoped<ICommandExecutor, CommandExecutor>();
    }
}
