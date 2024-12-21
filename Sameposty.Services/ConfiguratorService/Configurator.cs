using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Sameposty.Services.ConfiguratorService;
public class Configurator(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : IConfigurator
{
    public string ApiBaseUrl { get; private set; } = configuration.GetConnectionString("ApiBaseUrl") ?? throw new ArgumentNullException("No ApiBaseUrl provided in sppsettings.json");
    public string WwwRoot { get; private set; } = webHostEnvironment.WebRootPath;
    public int NumberFirstPostsGenerated { get; private set; } = configuration.GetValue<int>("Settings:NumberFirstPostsGenerated");
    public int NumberPremiumPostsGenerated { get; private set; } = configuration.GetValue<int>("Settings:NumberPremiumPostsGenerated");
    public int ImageTokensDefaultLimit { get; private set; } = configuration.GetValue<int>("Settings:ImageTokensDefaultLimit");
    public int TextTokensDefaultLimit { get; private set; } = configuration.GetValue<int>("Settings:TextTokensDefaultLimit");
    public string AngularClientBaseURl { get; private set; } = configuration.GetConnectionString("AngularClientBaseURl") ?? throw new ArgumentNullException("No AngularClientBaseURl provided in appsettings.json");
    public int ImageTokensPremiumLimit { get; private set; } = configuration.GetValue<int>("Settings:ImageTokensPremiumLimit");
    public int TextTokensPremiumLimit { get; private set; } = configuration.GetValue<int>("Settings:TextTokensPremiumLimit");
    public string StripeSubscriptionPriceId { get; private set; } = configuration.GetValue<string>("StripeSettings:StripeSubscriptionPriceId") ?? throw new ArgumentNullException("No StripeSubscriptionPriceId provided in sppsettings.json");
    public string StripeCouponId { get; private set; } = configuration.GetValue<string>("StripeSettings:StripeCouponId") ?? throw new ArgumentNullException("No StripeCouponId provided in sppsettings.json");
    public string SubscriptionSuccessPaymentUrl { get; private set; } = configuration.GetConnectionString("SubscriptionSuccessPaymentUrl") ?? throw new ArgumentNullException("No SubscriptionSuccessPaymentUrl provided in sppsettings.json");
    public string SubscriptionFailedPaymentUrl { get; private set; } = configuration.GetConnectionString("SubscriptionFailedPaymentUrl") ?? throw new ArgumentNullException("No SubscriptionFailedPaymentUrl provided in sppsettings.json");
    public int PostsDefaultLimit { get; private set; } = configuration.GetValue<int>("Settings:PostsDefaultLimit");
    public bool IsDevelopment => webHostEnvironment.IsDevelopment();
}
