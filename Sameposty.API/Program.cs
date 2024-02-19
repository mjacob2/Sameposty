using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using OpenAI.Extensions;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.ExampleS;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.PostDescriptionGenerator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(
        builder =>
        {
            builder
            .WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();
        });
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services
    .AddFastEndpoints()
    .AddJWTBearerAuth("hfgrtgdhgjhtudfghtyrewnhfjejwhufgdhgufghidgjid")
    .AddAuthorization()
    .SwaggerDocument();
builder.Services.AddDbContext<SamepostyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddTransient<IQueryExecutor, QueryExecutor>();
builder.Services.AddTransient<ICommandExecutor, CommandExecutor>();
builder.Services.AddScoped<IPostsGenerator, PostsGenerator>();
builder.Services.AddOpenAIService();
builder.Services.AddScoped<IImageGenerator, ImageGenerator>();
builder.Services.AddScoped<IPostDescriptionGenerator, PostDescriptionGenerator>();
builder.Services.AddScoped<IImageSaver>(sp =>
{
    var webHostEnvironment = sp.GetRequiredService<IWebHostEnvironment>();
    string wwwrootPath = webHostEnvironment.WebRootPath;
    return new ImageSaver(wwwrootPath, sp.GetService<HttpClient>());
});
builder.Services.AddScoped<IImageGeneratingOrchestrator, ImageGeneratingOrchestrator>();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IExample, Example>();

var app = builder.Build();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints()
    .UseSwaggerGen();


app.Run();
