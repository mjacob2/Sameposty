using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FastEndpoints;
using FastEndpoints.Swagger;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using OpenAI.Extensions;
using Sameposty.API;
using Sameposty.DataAccess;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.Services;
using Sameposty.Services.SecretsService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(
        corsbuilder =>
        {
            corsbuilder
            .WithOrigins(builder.Configuration.GetConnectionString("AngularClientBaseURl") ?? throw new ArgumentNullException("AngularClientBaseURl not provided"))
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();
        });
});

var secrets = new Secrets();
var dbConnectionString = "";

if (builder.Environment.IsDevelopment())
{
    secrets = Sameposty.API.DependencyInjection.AssignSecretsForDev(builder);
    dbConnectionString = builder.Configuration.GetConnectionString("LocalDbConnection")
        ?? throw new ArgumentNullException("No LocalDbConnection provided in appsettings.json");
}

if (builder.Environment.IsProduction())
{
    Uri keyVaultUrl = new(builder.Configuration.GetConnectionString("KeyVaultURL")
        ?? throw new ArgumentNullException("KeyVaultURL not provided"));
    var credential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, credential);
    var client = new SecretClient(keyVaultUrl, credential);

    dbConnectionString = client.GetSecret("ProductionDbConnectionString").Value.Value
        ?? throw new ArgumentNullException("No DbConnectionString provided in Azure Key Vault");

    secrets = Sameposty.API.DependencyInjection.AssignSecretsForProd(client);
}

builder.Services.AddDbContext<SamepostyDbContext>(options =>
            options.UseSqlServer(dbConnectionString, options =>
            {
                options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            }));

builder.Services.AddFastEndpoints(secrets.JWTBearerTokenSignKey);
builder.Services.AddSamepostyServices(secrets);
builder.Services.AddSamepostyDataAccess();

builder.Services.AddOpenAIService(settings => { settings.ApiKey = secrets.OpenAiApiKey; });
builder.Services.AddHttpContextAccessor();
builder.Services.AddHangfire(config => config
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings(o => o.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .UseSqlServerStorage(dbConnectionString));
builder.Services.AddHangfireServer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SamepostyDbContext>();
    await new SeedDatabase(dbContext, scope.ServiceProvider.GetRequiredService<ISecretsProvider>()).Run();
}

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new MyAuthorizationFilter() }
});
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints()
    .UseSwaggerGen();

app.Run();