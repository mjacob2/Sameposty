using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Sameposty.Services.Secrets;

namespace Sameposty.Services.FacebookPixel;

public class FacebookPixelNotifier(HttpClient httpClient, ISecretsProvider secrets) : IFacebookPixelNotifier
{
    private const string API_URL = "https://graph.facebook.com";
    private const string API_VERSION = "v20.0";
    private const string ENDPOINT = "events";
    private const string EVENT_NAME = "Lead";
    private const string ACTION_SOURCE = "website";

    public async Task<string> NotifyNewLeadAsync(string userEmail)
    {
        var url = $"{API_URL}/{API_VERSION}/{secrets.FacebookPixelId}/{ENDPOINT}?access_token={secrets.FacebookPixelAccessToken}";

        var payload = new FacebookPixelPayload
        {
            Data =
                [
                    new EventData
                    {
                        EventName = EVENT_NAME,
                        EventTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        ActionSource = ACTION_SOURCE,
                        UserData = new UserData
                        {
                            EmailHashes =
                            [
                                HashEmail(userEmail)
                            ]
                        },
                    }
                ]
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);

        var responseString = await response.Content.ReadAsStringAsync();

        return responseString;
    }

    private static string HashEmail(string email)
    {
        var bytes = Encoding.UTF8.GetBytes(email);
        var hash = SHA256.HashData(bytes);
        return string.Join("", hash.Select(b => b.ToString("x2")));
    }
}
