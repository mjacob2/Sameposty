using Newtonsoft.Json;

namespace Sameposty.Services.Fakturownia;
public class AddFakturowniaClientRequest
{
    [JsonProperty("api_token")]
    public string ApiToken { get; set; }

    [JsonProperty("client")]
    public AddFakturowniaClientModel Client { get; set; }
}

public class AddFakturowniaClientModel
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("tax_no")]
    public string NIP { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("post_code")]
    public string PostCode { get; set; }

    [JsonProperty("street")]
    public string Street { get; set; }
}
