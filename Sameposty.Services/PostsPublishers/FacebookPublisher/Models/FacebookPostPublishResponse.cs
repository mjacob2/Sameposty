using System.Text.Json.Serialization;

namespace Sameposty.Services.PostsPublishers.FacebookPublisher.Models;
public class FacebookPostPublishResponse
{
    /// <summary>
    /// Publishet Photo Id.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }


    /// <summary>
    /// Published Post Id.
    /// </summary>
    [JsonPropertyName("post_id")]
    public string PostId { get; set; }
}
