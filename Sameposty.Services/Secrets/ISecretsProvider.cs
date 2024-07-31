namespace Sameposty.Services.Secrets;

public interface ISecretsProvider
{
    string AdminPassword { get; }

    string StripeApiKey { get; }

    string JWTBearerTokenSignKey { get; }

    string StripeInvoicesWebhookKey { get; }

    string StripeSubscriptionsWebhookKey { get; }

    string FacebookPixelId { get; }

    string FacebookPixelAccessToken { get; }

    string SamepostyFacebookAppSecret { get; }

    string SamepostyFacebookAppId { get; }

    string EmailInfoPassword { get; }

    string FakturowniaApiKey { get; }
}
