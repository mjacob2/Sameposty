using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.Configurator;
using Sameposty.Services.FileRemover;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsPublishers.FacebookPublisher;
using Sameposty.Services.PostsPublishers.InstagramPublisher;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.PostsPublisher;

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
        services.AddScoped<ICommandExecutor, CommandExecutor>();
        services.AddScoped<IFileRemover, FileRemover>();
        services.AddScoped<IConfigurator, Configurator>();
        services.AddScoped<IImageSaver, ImageSaver>();
        services.AddHttpClient();
        services.AddScoped<IFacebookPublisher, FacebookPublisher>();
        services.AddScoped<IInstagramPublisher, InstagramPublisher>();
        services.AddScoped<IPostsPublisher, PostsPublisher>();
        services.AddScoped<IPostPublishOrchestrator, PostPublishOrchestrator>();
    })
    .Build();

host.Run();
