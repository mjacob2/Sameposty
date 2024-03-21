namespace Sameposty.DataAccess.Entities;

public class PublishResult : EntityBase
{
    public SocialMediaPlatform Platform { get; set; }

    public bool IsPublishedSuccess { get; set; }

    public string PublishedPostId { get; set; } = string.Empty;

    public string Error { get; set; } = string.Empty;

    public int UserId { get; set; }
}
