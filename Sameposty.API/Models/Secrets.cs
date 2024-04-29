namespace Sameposty.API.Models;

public class Secrets
{
    public string OpenAiApiKey { get; set; }

    public string JWTBearerTokenSignKey { get; set; }

    public string SamepostyFacebookAppSecret { get; set; }

    public string SamepostyFacebookAppId { get; set; }

    public string EmailInfoPassword { get; set; }

    public string StripeApiKey { get; set; }

    public string FakturowniaApiKey { get; set; }

    public string StripeSubscriptionsWebhookKey { get; set; }

    public string StripeInvoicesWebhookKey { get; set; }
}
