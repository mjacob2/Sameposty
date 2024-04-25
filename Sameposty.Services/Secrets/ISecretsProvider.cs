namespace Sameposty.Services.Secrets;
public interface ISecretsProvider
{
    string StripeApiKey { get; }

    string JWTBearerTokenSignKey { get; }
}
