using System.Text.Json.Serialization;

namespace Sameposty.Services.Fakturownia;
public class AddFakturowniaClientRequest
{
    [JsonPropertyName("api_token")]
    public string ApiToken { get; set; }

    [JsonPropertyName("client")]
    public AddFakturowniaClientModel Client { get; set; }
}

public class AddFakturowniaClientModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("tax_no")]
    public string NIP { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("post_code")]
    public string PostCode { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }
}
