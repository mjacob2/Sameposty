using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsPublishers.FacebookPublisher.Models;

namespace Sameposty.Services.PostsPublishers.FacebookPublisher;
public class FacebookPublisher(HttpClient http) : IFacebookPublisher
{
    private readonly string FacebookApiBaseUrl = "https://graph.facebook.com/v19.0";

    public async Task<PublishResult> PublishPost(Post post, FacebookConnection connection)
    {
        ArgumentNullException.ThrowIfNull(post);
        ArgumentNullException.ThrowIfNull(connection);

        var postToPublish = new FacegookPostToPublish()
        {
            ImageUrl = post.ImageUrl,
            Message = post.Description,
        };

        var pageInfo = new FacebookPageInfo()
        {
            LongLivedPageAccessToken = connection.AccesToken,
            PageId = connection.PageId,
        };

        string apiUrl = $"{FacebookApiBaseUrl}/{pageInfo.PageId}/photos";

        string jsonContent = JsonSerializer.Serialize(postToPublish);

        HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pageInfo.LongLivedPageAccessToken);

        var response = await http.PostAsync(apiUrl, httpContent);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<FacebookPostPublishResponse>(responseBody);
            string pagePostId = responseObject.PostId;
            http.DefaultRequestHeaders.Authorization = null;

            PublishResult result = new()
            {
                PublishedPostId = pagePostId,
                CreatedDate = DateTime.Now,
                Error = string.Empty,
                IsPublishedSuccess = true,
                Platform = SocialMediaPlatform.Facebook,
                UserId = post.UserId,
            };

            return result;
        }
        else
        {

            string responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<FacebookPostPublishErrorResponse>(responseBody);

            PublishResult result = new()
            {
                PublishedPostId = string.Empty,
                CreatedDate = DateTime.Now,
                Error = $"{responseObject.Error.ErrorUserTitle}, {responseObject.Error.ErrorUserMessage} {responseObject.Error.Message}" ?? "Wystapił niezidentyfikowany błąd!",
                IsPublishedSuccess = false,
                Platform = SocialMediaPlatform.Facebook,
                UserId = post.UserId,
            };

            return result;
        }
    }
}
