using System.Net.Http.Headers;
using System.Text.Json;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsPublishers.InstagramPublisher.Models;

namespace Sameposty.Services.PostsPublishers.InstagramPublisher;
public class InstagramPublisher(HttpClient http) : IInstagramPublisher
{
    private readonly string FacebookApiBaseUrl = "https://graph.facebook.com/v19.0";

    public async Task<PublishResult> PublishPost(Post post, InstagramConnection connection)
    {
        ArgumentNullException.ThrowIfNull(post);
        ArgumentNullException.ThrowIfNull(connection);

        var imgUrl = post.ImageUrl;
        var text = post.Description;

        string apiUrl = $"{FacebookApiBaseUrl}/{connection.PageId}/media?image_url={imgUrl}&caption={text}";

        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connection.AccesToken);

        var getContainerResposne = await http.PostAsync(apiUrl, null);

        if (getContainerResposne.IsSuccessStatusCode)
        {
            var getContainerResponseBody = await getContainerResposne.Content.ReadAsStringAsync();
            var getContainerResponseObject = JsonSerializer.Deserialize<ContainerReposne>(getContainerResponseBody);

            var apiUrlPublishContainer = $"{FacebookApiBaseUrl}/{connection.PageId}/media_publish?creation_id={getContainerResponseObject.Id}";

            var publishContainerResposne = await http.PostAsync(apiUrlPublishContainer, null);

            if (publishContainerResposne.IsSuccessStatusCode)
            {
                var publishContainerResposneBody = await publishContainerResposne.Content.ReadAsStringAsync();
                var publishContainerResposneObject = JsonSerializer.Deserialize<ContainerReposne>(publishContainerResposneBody);
                http.DefaultRequestHeaders.Authorization = null;

                PublishResult result = new()
                {
                    PublishedPostId = publishContainerResposneObject.Id,
                    CreatedDate = DateTime.Now,
                    Error = string.Empty,
                    IsPublishedSuccess = true,
                    Platform = SocialMediaPlatform.Instagram,
                    UserId = post.UserId,
                };

                return result;
            }
            else
            {

                var responseBody = await publishContainerResposne.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<InstagramErrorResponse>(responseBody);

                PublishResult result = new()
                {
                    PublishedPostId = string.Empty,
                    CreatedDate = DateTime.Now,
                    Error = $"Wystapił problem z opublikowaniem kontenera: {responseObject.Error.Message}" ?? "Wystapił niezidentyfikowany błąd!",
                    IsPublishedSuccess = true,
                    Platform = SocialMediaPlatform.Instagram,
                    UserId = post.UserId,
                };

                return result;
            }
        }
        else
        {
            var responseBody = await getContainerResposne.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<InstagramErrorResponse>(responseBody);

            PublishResult result = new()
            {
                PublishedPostId = string.Empty,
                CreatedDate = DateTime.Now,
                Error = $"{responseObject.Error.Message}" ?? "Wystapił niezidentyfikowany błąd!",
                IsPublishedSuccess = false,
                Platform = SocialMediaPlatform.Instagram,
                UserId = post.UserId,
            };

            return result;
        }
    }
}


