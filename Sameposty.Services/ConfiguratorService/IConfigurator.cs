﻿namespace Sameposty.Services.ConfiguratorService;
public interface IConfigurator
{
    public string ApiBaseUrl { get; }
    public string WwwRoot { get; }
    public int ImageTokensDefaultLimit { get; }
    public int TextTokensDefaultLimit { get; }
    public int ImageTokensPremiumLimit { get; }
    public int TextTokensPremiumLimit { get; }
    public string StripeSubscriptionPriceId { get; }
    public string StripeCouponId { get; }
    string AngularClientBaseURl { get; }
    string SubscriptionSuccessPaymentUrl { get; }
    string SubscriptionFailedPaymentUrl { get; }
    bool IsDevelopment { get; }
    int PostsDefaultLimit { get; }
    int PostsPremiumLimit { get; }
}
