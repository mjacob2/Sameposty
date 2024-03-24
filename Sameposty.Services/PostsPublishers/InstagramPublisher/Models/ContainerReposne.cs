using System.Text.Json.Serialization;

namespace Sameposty.Services.PostsPublishers.InstagramPublisher.Models;

public class ContainerReposne
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
