using System.Text.Json;
using Sameposty.Services.FacebookTokenManagerService.Models;
using Sameposty.Services.SecretsService;

namespace Sameposty.Services.FacebookTokenManagerService;
public class FacebookTokenManager(HttpClient http, ISecretsProvider secrets) : IFacebookTokenManager
{
    private readonly string FacebookApiBaseUrl = "https://graph.facebook.com/v19.0/";

    public async Task<string> GetLongLivedPageAccessToken(string longLivedUserAccessToken, string facebookUserId, string pageId)
    {
        string apiUrl = $"{FacebookApiBaseUrl}/{facebookUserId}/accounts";
        string queryString = $"access_token={longLivedUserAccessToken}";
        string fullUrl = $"{apiUrl}?{queryString}";

        var response = await http.GetAsync(fullUrl);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonSerializer.Deserialize<FacebookPageAccessTokenResponse>(responseBody);

            string longLivedPageAccessToken = responseObject.Data.FirstOrDefault(x => x.Id == pageId).AccessToken;

            return longLivedPageAccessToken;
        }
        else
        {
            throw new HttpRequestException($"Error while getting Facebook PAge Long Lived Access token: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }

    public async Task<string> GetLongLivedUserAccessToken(string shortLivedUserAccessToken)
    {
        string apiUrl = $"{FacebookApiBaseUrl}oauth/access_token";
        string queryString = $"grant_type=fb_exchange_token&client_id={secrets.SamepostyFacebookAppId}&client_secret={secrets.SamepostyFacebookAppSecret}&fb_exchange_token={shortLivedUserAccessToken}";
        string fullUrl = $"{apiUrl}?{queryString}";

        var response = await http.GetAsync(fullUrl);
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(responseBody);
            string longLivedUserAccestoen = responseObject.AccessToken;

            return longLivedUserAccestoen;
        }
        else
        {
            throw new HttpRequestException($"Error while getting Facebook User Long Lived Access token: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }
}
