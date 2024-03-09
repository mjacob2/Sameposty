using System.Text.Json.Serialization;
using static Sameposty.Services.FacebookTokenManager.FacebookTokenManager;

namespace Sameposty.Services.FacebookTokenManager.Models;
public class FacebookPageData
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("category_list")]
    public List<FacebookCategory> CategoryList { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("tasks")]
    public List<string> Tasks { get; set; }
}
