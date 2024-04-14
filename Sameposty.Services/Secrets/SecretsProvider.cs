namespace Sameposty.Services.Secrets;
public class SecretsProvider(string stripeApiKey) : ISecretsProvider
{
    public string StripeApiKey { get; private set; } = stripeApiKey;
}
