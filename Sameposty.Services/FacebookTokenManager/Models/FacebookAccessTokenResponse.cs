using System.Text.Json.Serialization;

namespace Sameposty.Services.FacebookTokenManager.Models;
public class FacebookAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public long ExpiresInSeconds { get; set; }
}
