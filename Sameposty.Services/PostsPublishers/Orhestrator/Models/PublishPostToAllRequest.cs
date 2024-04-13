using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.Orhestrator.Models;
public class PublishPostToAllRequest
{
    public string BaseApiUrl { get; set; }

    public Post Post { get; set; }
}