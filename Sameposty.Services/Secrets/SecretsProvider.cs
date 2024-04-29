namespace Sameposty.Services.Secrets;
public class SecretsProvider(string stripeApiKey, string JWTBearerTokenSignKey, string StripeInvoicesWebhookKey, string StripeSubscriptionsWebhookKey) : ISecretsProvider
{
    public string StripeApiKey { get; private set; } = stripeApiKey;

    public string JWTBearerTokenSignKey { get; private set; } = JWTBearerTokenSignKey;

    public string StripeInvoicesWebhookKey { get; private set; } = StripeInvoicesWebhookKey;

    public string StripeSubscriptionsWebhookKey { get; private set; } = StripeSubscriptionsWebhookKey;
}
