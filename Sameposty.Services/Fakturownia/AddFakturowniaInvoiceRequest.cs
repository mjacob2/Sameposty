using System.Text.Json.Serialization;

namespace Sameposty.Services.Fakturownia;
public class AddFakturowniaInvoiceRequest()
{
    [JsonPropertyName("api_token")]
    public string ApiToken { get; set; }

    [JsonPropertyName("invoice")]
    public AddFakturowniaInvoiceModel Invoice { get; set; }
}

public class AddFakturowniaInvoiceModel(long? clientId, double price)
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; } = "vat";

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
    public FakturowniaPositions Positions { get; set; } = new(price);

    [JsonPropertyName("split_payment")]
    public string SplitPayment { get; set; } = "0";
}

public class FakturowniaPositions(double price)
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "SamePosty.pl subskrypcja premium";

    [JsonPropertyName("tax")]
    public string Tax { get; set; } = "zw";

    [JsonPropertyName("total_price_gross")]
    public double Price { get; set; } = price;

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; } = 1;
}