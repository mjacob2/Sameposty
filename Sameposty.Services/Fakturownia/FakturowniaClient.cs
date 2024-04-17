using System.Text.Json.Serialization;

namespace Sameposty.Services.Fakturownia;
public class FakturowniaClient
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}
