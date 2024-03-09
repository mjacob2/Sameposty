using System.Text.Json.Serialization;

namespace Sameposty.Services.FacebookTokenManager.Models;
public class FacebookAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}
