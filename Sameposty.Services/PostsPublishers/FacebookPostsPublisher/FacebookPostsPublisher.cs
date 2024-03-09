using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Sameposty.Services.PostsPublishers.FacebookPostsPublisher.Models;

namespace Sameposty.Services.PostsPublishers.FacebookPostsPublisher;
public class FacebookPostsPublisher(HttpClient http) : IFacebookPostsPublisher
{
    private readonly string FacebookApiBaseUrl = "https://graph.facebook.com/v19.0/";

    public async Task<string> PublishPost(FacegookPostToPublish post, FacebookPageInfo pageInfo)
    {
        ArgumentNullException.ThrowIfNull(post);

        string apiUrl = $"{FacebookApiBaseUrl}{pageInfo.PageId}/photos";

        string jsonContent = JsonSerializer.Serialize(post);

        HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pageInfo.LongLivedPageAccessToken);

        var response = await http.PostAsync(apiUrl, httpContent);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<FacebookPostPublishResponse>(responseBody);
            string pagePostId = responseObject.PostId;
            http.DefaultRequestHeaders.Authorization = null;
            return pagePostId;
        }
        else
        {
            throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }
}
