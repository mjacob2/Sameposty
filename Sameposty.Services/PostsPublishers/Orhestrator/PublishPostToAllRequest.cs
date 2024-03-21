using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.Orhestrator;
public class PublishPostToAllRequest
{
    public string BaseApiUrl { get; set; }

    public Post Post { get; set; }

    public ConnectionsModel Connections { get; set; }
}