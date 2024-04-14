using Newtonsoft.Json;

namespace Sameposty.Services.Fakturownia;
public class AddFakturowniaInvoiceRequest
{
    [JsonProperty("api_token")]
    public string ApiToken { get; set; }

    [JsonProperty("invoice")]
    public AddFakturowniaInvoiceModel Invoice { get; set; }
}

public class AddFakturowniaInvoiceModel(long clientId)
{
    [JsonProperty("kind")]
    public string Kind { get; set; } = "vat";

    [JsonProperty("income")]
    public string Income { get; set; } = "1";


    //[JsonProperty("issue_date")]
    //public string IssueDate { get; set; } = DateTime.Today.ToString();

    [JsonProperty("place")]
    public string Place { get; set; } = "Wrocław";

    //[JsonProperty("sell_date")]
    //public string SellDate { get; set; } = DateTime.Today.ToString();

    [JsonProperty("client_id")]
    public long ClientId { get; set; } = clientId;

    [JsonProperty("payment_type")]
    public string PaymentType { get; set; } = "card";

    [JsonProperty("payment_to_kind")]
    public string PaymentToKind { get; set; } = "off";

    [JsonProperty("status")]
    public string Status { get; set; } = "paid";

    //[JsonProperty("paid")]
   // public string Paid { get; set; } = amounPaid;

    [JsonProperty("positions")]
    public FakturowniaPositions Positions { get; set; }


    [JsonProperty("split_payment")]
    public string SplitPayment { get; set; } = "0";
}

public class FakturowniaPositions
{
    [JsonProperty("product_id")]
    public long ProductId { get; set; } = 1099106483;

    [JsonProperty("quantity")]
    public int Quantity { get; set; } = 1;
}