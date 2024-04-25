namespace Sameposty.Services.Secrets;
public class SecretsProvider(string stripeApiKey, string JWTBearerTokenSignKey) : ISecretsProvider
{
    public string StripeApiKey { get; private set; } = stripeApiKey;

    public string JWTBearerTokenSignKey { get; private set; } = JWTBearerTokenSignKey;
}
