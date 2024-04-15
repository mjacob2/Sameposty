using System.Text.Json.Serialization;

namespace Sameposty.API.Endpoints.StripeWebhook;

public class StripeWebhookInvoicesRequest
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }
}

public class Data
{
    [JsonPropertyName("object")]
    public SripeInvoice StripeInvoice { get; set; }

}

public class SripeInvoice
{
    [JsonPropertyName("customer")]
    public string StripeCustomerId { get; set; }

    [JsonPropertyName("customer_email")]
    public string Email { get; set; }

    [JsonPropertyName("subscription")]
    public string StripeSubscriptionId { get; set; }
}




