using System.Text.Json.Serialization;

namespace Sameposty.Services.FacebookTokenManager.Models;
public class FacebookPageAccessTokenResponse
{
    [JsonPropertyName("data")]
    public List<FacebookPageData> Data { get; set; }
}
