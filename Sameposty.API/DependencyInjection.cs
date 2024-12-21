using Azure.Security.KeyVault.Secrets;
using FastEndpoints;
using Sameposty.Services.SecretsService;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

namespace Sameposty.API;

public static class DependencyInjection
{
    public static Secrets AssignSecretsForDev(WebApplicationBuilder builder)
    {
        return builder.Configuration.GetSection("Secrets").Get<Secrets>()
            ?? throw new ArgumentNullException("No local secrets.json provided or error while mapping");
    }

    public static Secrets AssignSecretsForProd(SecretClient client)
    {
        var secrets = new Secrets
        {
            AdminPassword = client.GetSecret("AdminPassword").Value.Value
            ?? throw new ArgumentNullException("No AdminPassword provided in Azure Key Vault"),

            OpenAiApiKey = client.GetSecret("OpenAiApiKey").Value.Value
            ?? throw new ArgumentNullException("No OpenAiApiKey provided in Azure Key Vault"),

            JWTBearerTokenSignKey = client.GetSecret("JWTBearerTokenSignKey").Value.Value
            ?? throw new ArgumentNullException("No JWTBearerTokenSignKey provided in Azure Key Vault"),

            SamepostyFacebookAppSecret = client.GetSecret("SamepostyFacebookAppSecret").Value.Value
            ?? throw new ArgumentNullException("No SamepostyFacebookAppSecret provided in Azure Key Vault"),

            SamepostyFacebookAppId = client.GetSecret("SamepostyFacebookAppId").Value.Value
            ?? throw new ArgumentNullException("No SamepostyFacebookAppId provided in Azure Key Vault"),

            EmailInfoPassword = client.GetSecret("EmailInfoPassword").Value.Value
            ?? throw new ArgumentNullException("No EmailInfoPassword provided in Azure Key Vault"),

            StripeApiKey = client.GetSecret("StripeApiKey").Value.Value
            ?? throw new ArgumentNullException("No StripeApiKey provided in Azure Key Vault"),

            FakturowniaApiKey = client.GetSecret("FakturowniaApiKey").Value.Value
            ?? throw new ArgumentNullException("No FakturowniaApiKey provided in Azure Key Vault"),

            StripeSubscriptionsWebhookKey = client.GetSecret("StripeSubscriptionsWebhookKey").Value.Value
            ?? throw new ArgumentNullException("No StripeSubscriptionsWebhookKey provided in Azure Key Vault"),

            StripeInvoicesWebhookKey = client.GetSecret("StripeInvoicesWebhookKey").Value.Value
            ?? throw new ArgumentNullException("No StripeInvoicesWebhookKey provided in Azure Key Vault"),

            FacebookPixelId = client.GetSecret("FacebookPixelId").Value.Value
            ?? throw new ArgumentNullException("No FacebookPixelId provided in Azure Key Vault"),

            FacebookPixelAccessToken = client.GetSecret("FacebookPixelAccessToken").Value.Value
            ?? throw new ArgumentNullException("No FacebookPixelAccessToken provided in Azure Key Vault")
        };

        return secrets;
    }

    public static void AddFastEndpoints(this IServiceCollection services, string key)
    {
        services.AddAuthenticationJwtBearer(s => s.SigningKey = key);
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", x => x.RequireRole("Admin"));
        services.AddFastEndpoints();
        services.SwaggerDocument();
    }
}
