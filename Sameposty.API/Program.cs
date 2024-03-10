using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using OpenAI.Extensions;
using Sameposty.API.Models;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.FacebookTokenManager;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;
using Sameposty.Services.PostsPublishers.FacebookPostsPublisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(
        corsbuilder =>
        {
            corsbuilder
            .WithOrigins(builder.Configuration.GetConnectionString("AngularClientBaseUrlProduction")
            ?? throw new ArgumentNullException("AngularClientBaseUrlProduction not provided"), builder.Configuration.GetConnectionString("AngularClientBaseUrlDevelopment")
            ?? throw new ArgumentNullException("AngularClientBaseUrlDevelopment not provided"))
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();
        });
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

var secrets = new Secrets();
var dbConnectionString = "";
var baseApiUrl = "";

if (builder.Environment.IsDevelopment())
{
    secrets = builder.Configuration.GetSection("Secrets").Get<Secrets>() ?? throw new ArgumentNullException("No local secrets.json provided or error while mapping");
    dbConnectionString = builder.Configuration.GetConnectionString("LocalDbConnection") ?? throw new ArgumentNullException("No LocalDbConnection provided in sppsettings.json");
    baseApiUrl = "";
}

if (builder.Environment.IsProduction())
{
    Uri keyVaultUrl = new(builder.Configuration.GetConnectionString("KeyVaultURL") ?? throw new ArgumentNullException("KeyVaultURL not provided"));
    var credential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, credential);
    var client = new SecretClient(keyVaultUrl, credential);

    dbConnectionString = client.GetSecret("ProductionDbConnectionString").Value.Value ?? throw new ArgumentNullException("No DbConnectionString provided in Azure Key Vault");
    secrets.OpenAiApiKey = client.GetSecret("OpenAiApiKey").Value.Value ?? throw new ArgumentNullException("No OpenAiApiKey provided in Azure Key Vault");
    secrets.JWTBearerTokenSignKey = client.GetSecret("JWTBearerTokenSignKey").Value.Value ?? throw new ArgumentNullException("No JWTBearerTokenSignKey provided in Azure Key Vault");
    secrets.SamepostyFacebookAppSecret = client.GetSecret("SamepostyFacebookAppSecret").Value.Value ?? throw new ArgumentNullException("No SamepostyFacebookAppSecret provided in Azure Key Vault");
    secrets.SamepostyFacebookAppId = client.GetSecret("SamepostyFacebookAppId").Value.Value ?? throw new ArgumentNullException("No SamepostyFacebookAppId provided in Azure Key Vault");
}

builder.Services.AddDbContext<SamepostyDbContext>(options =>
            options.UseSqlServer(dbConnectionString));

AddFastEndpoints(builder, secrets.JWTBearerTokenSignKey);

builder.Services.AddTransient<IQueryExecutor, QueryExecutor>();
builder.Services.AddTransient<ICommandExecutor, CommandExecutor>();
builder.Services.AddScoped<IPostsGenerator>(sp =>
{
    var apiBaseUrl = builder.Configuration.GetConnectionString("ApiBaseUrl") ?? throw new ArgumentNullException("No ApiBaseUrl provided in sppsettings.json");
    return new PostsGenerator(sp.GetRequiredService<ITextGenerator>(), sp.GetRequiredService<IImageGeneratingOrchestrator>(), apiBaseUrl);
});
builder.Services.AddOpenAIService(settings => { settings.ApiKey = secrets.OpenAiApiKey; });

builder.Services.AddScoped<IImageGenerator, ImageGenerator>();

builder.Services.AddScoped<ITextGenerator, TextGenerator>();
AddImageServer(builder);
builder.Services.AddScoped<IImageGeneratingOrchestrator, ImageGeneratingOrchestrator>();
builder.Services.AddScoped<IFacebookTokenManager>(options =>
{
    var s = new FacebookTokenManagerSecrets()
    {
        SamepostyFacebookAppId = secrets.SamepostyFacebookAppId,
        SamepostyFacebookAppSecret = secrets.SamepostyFacebookAppSecret,
    };

    return new FacebookTokenManager(s, options.GetRequiredService<HttpClient>());
});

builder.Services.AddScoped<IFacebookPostsPublisher, FacebookPostsPublisher>();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints()
    .UseSwaggerGen();


app.Run();

static void AddImageServer(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IImageSaver>(sp =>
    {
        var webHostEnvironment = sp.GetRequiredService<IWebHostEnvironment>();
        string wwwrootPath = webHostEnvironment.WebRootPath;
        return new ImageSaver(wwwrootPath, sp.GetRequiredService<HttpClient>());
    });
}

static void AddFastEndpoints(WebApplicationBuilder builder, string key)
{
    builder.Services
        .AddFastEndpoints()
        .AddJWTBearerAuth(key)
        .AddAuthorization()
        .SwaggerDocument();
}