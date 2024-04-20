using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Executors;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

        //services.AddApplicationInsightsTelemetryWorkerService();
        //services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<SamepostyDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<IQueryExecutor, QueryExecutor>();
    })
    .Build();

host.Run();
