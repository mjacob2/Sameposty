using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;

public class PublishResult : EntityBase
{
    public SocialMediaPlatform Platform { get; set; }

    public bool IsPublishedSuccess { get; set; }

    public string PublishedPostId { get; set; } = string.Empty;

    public string Error { get; set; } = string.Empty;

    [JsonIgnore]
    public Post User { get; set; }

    public int UserId { get; set; }
}
