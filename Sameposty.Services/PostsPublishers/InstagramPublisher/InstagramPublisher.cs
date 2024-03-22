using System.Net.Http.Headers;
using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.InstagramPublisher;
public class InstagramPublisher(HttpClient http) : IInstagramPublisher
{
    private readonly string FacebookApiBaseUrl = "https://graph.facebook.com/v19.0";

    public async Task<PublishResult> PublishPost(Post post, InstagramConnection connection)
    {
        ArgumentNullException.ThrowIfNull(post);
        ArgumentNullException.ThrowIfNull(connection);

        var imgUrl = "https://middlers.pl/images/Marek-Jakubicki-doradca-kredytowy-Wroclaw-2.png";
        var text = "This is sample text";

        string apiUrl = $"{FacebookApiBaseUrl}/{connection.PageId}/media?image_url={imgUrl}&caption={text}";

        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connection.AccesToken);

        var response = await http.PostAsync(apiUrl, null);

        string responseBody = await response.Content.ReadAsStringAsync();

        return new PublishResult();
    }
}
