using Microsoft.Extensions.DependencyInjection;
using Sameposty.Services.FacebookPixel;
using Sameposty.Services.FileRemoverService;
using Sameposty.Services.PostsGeneratorService;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.TextGenerator;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.EmailManager;
using Sameposty.Services.FacebookTokenManagerService;
using Sameposty.Services.PostsPublishers.FacebookPublisher;
using Sameposty.Services.PostsPublishers.InstagramPublisher;
using Sameposty.Services.PostsPublishers.PostsPublisher;
using Sameposty.Services.ConfiguratorService;
using Sameposty.Services.StripeWebhooksManagers.Subscriptions;
using Sameposty.Services.StripeWebhooksManagers;
using Sameposty.Services.Fakturownia;
using Sameposty.Services.JWTService;
using Sameposty.Services.PostGeneratingManager;
using Sameposty.Services.REGON;
using Sameposty.Services.StripeServices;
using Sameposty.Services.SubscriptionManager;
using Sameposty.Services.SecretsService;

namespace Sameposty.Services;
public static class DependencyInjection
{
    public static void AddSamepostyServices(this IServiceCollection services, Secrets secrets)
    {
        services.AddScoped<IFacebookPixelNotifier, FacebookPixelNotifier>();
        services.AddScoped<IPostsGenerator, PostsGenerator>();
        services.AddScoped<IPostPublishOrchestrator, PostPublishOrchestrator>();
        services.AddScoped<IImageGenerator, ImageGenerator>();
        services.AddScoped<ITextGenerator, TextGenerator>();
        services.AddScoped<IImageSaver, ImageSaver>();
        services.AddScoped<IFileRemover, FileRemover>();
        services.AddScoped<IImageGeneratingOrchestrator, ImageGeneratingOrchestrator>();
        services.AddScoped<IFacebookTokenManager, FacebookTokenManager>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFacebookPublisher, FacebookPublisher>();
        services.AddScoped<IInstagramPublisher, InstagramPublisher>();
        services.AddScoped<IPostsPublisher, PostsPublisher>();
        services.AddHttpClient();
        services.AddScoped<IConfigurator, Configurator>();
        services.AddScoped<IStripeWebhooksManager, StripeInvoiceWebhooksManager>();
        services.AddScoped<IStripeSubscriptionWebhooksManager, StripeSubscriptionWebhooksManager>();
        services.AddScoped<IRegonService, RegonService>();
        services.AddScoped<IStripeService, StripeManager>();
        services.AddScoped<ISubscriptionManager, SubscriptionService>();
        services.AddSingleton<IFakturowniaService, FakturowniaService>();
        services.AddSingleton<IJWTBearerProvider, JWTBearerProvider>();
        services.AddTransient<IPostGeneratingManager, PostGeneratingService>();
        services.AddSingleton<ISecretsProvider>(_ =>
        {
            return new SecretsProvider(secrets);
        });
    }
}
