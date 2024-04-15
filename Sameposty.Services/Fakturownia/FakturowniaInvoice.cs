using System.Text.Json.Serialization;

namespace Sameposty.Services.Fakturownia;
public class FakturowniaInvoice
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("issue_date")]
    public string IssueDate { get; set; }

    [JsonPropertyName("price_net")]
    public string PriceNet { get; set; }

    [JsonPropertyName("price_gross")]
    public string PriceGRoss { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("client_id")]
    public string FakturowniaClientId { get; set; }
}
