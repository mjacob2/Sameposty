using System.Text.Json.Serialization;

namespace Sameposty.Services.FacebookTokenManager.Models;
public class FacebookCategory
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
