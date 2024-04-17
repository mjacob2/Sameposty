using System.Text.Json.Serialization;

namespace Sameposty.Services.Fakturownia;
public class AddFakturowniaInvoiceRequest
{
    [JsonPropertyName("api_token")]
    public string ApiToken { get; set; }

    [JsonPropertyName("invoice")]
    public AddFakturowniaInvoiceModel Invoice { get; set; }
}

public class AddFakturowniaInvoiceModel(long? clientId)
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; } = "vat";

    [JsonPropertyName("income")]
    public string Income { get; set; } = "1";

    [JsonPropertyName("place")]
    public string Place { get; set; } = "Wrocław";

    [JsonPropertyName("client_id")]
    public long? ClientId { get; set; } = clientId;

    [JsonPropertyName("payment_type")]
    public string PaymentType { get; set; } = "card";

    [JsonPropertyName("payment_to_kind")]
    public string PaymentToKind { get; set; } = "off";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "paid";

    [JsonPropertyName("positions")]
    public FakturowniaPositions Positions { get; set; } = new();

    [JsonPropertyName("split_payment")]
    public string SplitPayment { get; set; } = "0";
}

public class FakturowniaPositions
{
    [JsonPropertyName("product_id")]
    public long ProductId { get; set; } = 1099106483;

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; } = 1;
}