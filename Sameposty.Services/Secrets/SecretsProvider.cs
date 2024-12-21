namespace Sameposty.Services.SecretsService;
public class SecretsProvider(Secrets secrets) : ISecretsProvider
{
    public string AdminPassword { get; private set; } = secrets.AdminPassword;

    public string StripeApiKey { get; private set; } = secrets.StripeApiKey;

    public string JWTBearerTokenSignKey { get; private set; } = secrets.JWTBearerTokenSignKey;

    public string StripeInvoicesWebhookKey { get; private set; } = secrets.StripeInvoicesWebhookKey;

    public string StripeSubscriptionsWebhookKey { get; private set; } = secrets.StripeSubscriptionsWebhookKey;

    public string FacebookPixelId { get; private set; } = secrets.FacebookPixelId;

    public string FacebookPixelAccessToken { get; private set; } = secrets.FacebookPixelAccessToken;

    public string SamepostyFacebookAppSecret { get; private set; } = secrets.SamepostyFacebookAppSecret;

    public string SamepostyFacebookAppId { get; private set; } = secrets.SamepostyFacebookAppId;

    public string EmailInfoPassword { get; private set; } = secrets.EmailInfoPassword;

    public string FakturowniaApiKey { get; private set; } = secrets.FakturowniaApiKey;

}
