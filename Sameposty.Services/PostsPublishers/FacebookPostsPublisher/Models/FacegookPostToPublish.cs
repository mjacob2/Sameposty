using System.Text.Json.Serialization;

namespace Sameposty.Services.PostsPublishers.FacebookPostsPublisher.Models;
public class FacegookPostToPublish
{
    [JsonPropertyName("url")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
