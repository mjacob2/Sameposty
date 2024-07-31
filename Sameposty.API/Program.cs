using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using OpenAI.Extensions;
using Sameposty.API;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.FacebookPixel;
using Sameposty.Services.FacebookTokenManager;
using Sameposty.Services.Fakturownia;
using Sameposty.Services.FileRemover;
using Sameposty.Services.JWTService;
using Sameposty.Services.PostGeneratingManager;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;
using Sameposty.Services.PostsPublishers.FacebookPublisher;
using Sameposty.Services.PostsPublishers.InstagramPublisher;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.PostsPublisher;
using Sameposty.Services.REGON;
using Sameposty.Services.Secrets;
using Sameposty.Services.StripeServices;
using Sameposty.Services.StripeWebhooksManagers;
using Sameposty.Services.StripeWebhooksManagers.Subscriptions;
using Sameposty.Services.SubscriptionManager;

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
    secrets = builder.Configuration.GetSection("Secrets").Get<Secrets>() ?? throw new ArgumentNullException("No local secrets.json provided or error while mapping");
    dbConnectionString = builder.Configuration.GetConnectionString("LocalDbConnection") ?? throw new ArgumentNullException("No LocalDbConnection provided in sppsettings.json");
}

if (builder.Environment.IsProduction())
{
    Uri keyVaultUrl = new(builder.Configuration.GetConnectionString("KeyVaultURL") ?? throw new ArgumentNullException("KeyVaultURL not provided"));
    var credential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, credential);
    var client = new SecretClient(keyVaultUrl, credential);

    dbConnectionString = client.GetSecret("ProductionDbConnectionString").Value.Value ?? throw new ArgumentNullException("No DbConnectionString provided in Azure Key Vault");

    secrets.AdminPassword = client.GetSecret("AdminPassword").Value.Value ?? throw new ArgumentNullException("No AdminPassword provided in Azure Key Vault");
    secrets.OpenAiApiKey = client.GetSecret("OpenAiApiKey").Value.Value ?? throw new ArgumentNullException("No OpenAiApiKey provided in Azure Key Vault");
    secrets.JWTBearerTokenSignKey = client.GetSecret("JWTBearerTokenSignKey").Value.Value ?? throw new ArgumentNullException("No JWTBearerTokenSignKey provided in Azure Key Vault");
    secrets.SamepostyFacebookAppSecret = client.GetSecret("SamepostyFacebookAppSecret").Value.Value ?? throw new ArgumentNullException("No SamepostyFacebookAppSecret provided in Azure Key Vault");
    secrets.SamepostyFacebookAppId = client.GetSecret("SamepostyFacebookAppId").Value.Value ?? throw new ArgumentNullException("No SamepostyFacebookAppId provided in Azure Key Vault");
    secrets.EmailInfoPassword = client.GetSecret("EmailInfoPassword").Value.Value ?? throw new ArgumentNullException("No EmailInfoPassword provided in Azure Key Vault");
    secrets.StripeApiKey = client.GetSecret("StripeApiKey").Value.Value ?? throw new ArgumentNullException("No StripeApiKey provided in Azure Key Vault");
    secrets.FakturowniaApiKey = client.GetSecret("FakturowniaApiKey").Value.Value ?? throw new ArgumentNullException("No FakturowniaApiKey provided in Azure Key Vault");
    secrets.StripeSubscriptionsWebhookKey = client.GetSecret("StripeSubscriptionsWebhookKey").Value.Value ?? throw new ArgumentNullException("No StripeSubscriptionsWebhookKey provided in Azure Key Vault");
    secrets.StripeInvoicesWebhookKey = client.GetSecret("StripeInvoicesWebhookKey").Value.Value ?? throw new ArgumentNullException("No StripeInvoicesWebhookKey provided in Azure Key Vault");
    secrets.FacebookPixelId = client.GetSecret("FacebookPixelId").Value.Value ?? throw new ArgumentNullException("No FacebookPixelId provided in Azure Key Vault");
    secrets.FacebookPixelAccessToken = client.GetSecret("FacebookPixelAccessToken").Value.Value ?? throw new ArgumentNullException("No FacebookPixelAccessToken provided in Azure Key Vault");
}

AddFastEndpoints(builder, secrets.JWTBearerTokenSignKey);

builder.Services.AddDbContext<SamepostyDbContext>(options =>
            options.UseSqlServer(dbConnectionString));

builder.Services.AddSingleton<ISecretsProvider>(_ =>
{
    return new SecretsProvider(secrets);
});

builder.Services.AddScoped<IFacebookPixelNotifier, FacebookPixelNotifier>();
builder.Services.AddScoped<IQueryExecutor, QueryExecutor>();
builder.Services.AddScoped<ICommandExecutor, CommandExecutor>();
builder.Services.AddScoped<IPostsGenerator, PostsGenerator>();
builder.Services.AddScoped<IPostPublishOrchestrator, PostPublishOrchestrator>();
builder.Services.AddOpenAIService(settings => { settings.ApiKey = secrets.OpenAiApiKey; });
builder.Services.AddScoped<IImageGenerator, ImageGenerator>();
builder.Services.AddScoped<ITextGenerator, TextGenerator>();
builder.Services.AddScoped<IImageSaver, ImageSaver>();
builder.Services.AddScoped<IFileRemover, FileRemover>();
builder.Services.AddScoped<IImageGeneratingOrchestrator, ImageGeneratingOrchestrator>();
builder.Services.AddScoped<IFacebookTokenManager, FacebookTokenManager>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFacebookPublisher, FacebookPublisher>();
builder.Services.AddScoped<IInstagramPublisher, InstagramPublisher>();
builder.Services.AddScoped<IPostsPublisher, PostsPublisher>();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IConfigurator, Configurator>();
builder.Services.AddScoped<IStripeWebhooksManager, StripeInvoiceWebhooksManager>();
builder.Services.AddScoped<IStripeSubscriptionWebhooksManager, StripeSubscriptionWebhooksManager>();
builder.Services.AddHangfire(config => config
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings(o => o.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
.UseSqlServerStorage(dbConnectionString));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<IRegonService, RegonService>();
builder.Services.AddScoped<IStripeService, StripeServices>();
builder.Services.AddScoped<ISubscriptionManager, SubscriptionManager>();
builder.Services.AddSingleton<IFakturowniaService, FakturowniaService>();
builder.Services.AddSingleton<IJWTBearerProvider, JWTBearerProvider>();
builder.Services.AddTransient<IPostGeneratingManager, PostGeneratingManager>();

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

static void AddFastEndpoints(WebApplicationBuilder builder, string key)
{
    builder.Services.AddAuthenticationJwtBearer(s => s.SigningKey = key);
    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("AdminOnly", x => x.RequireRole("Admin"));
    builder.Services.AddFastEndpoints();
    builder.Services.SwaggerDocument();
}