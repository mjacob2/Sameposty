using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.Orhestrator;
public interface IPostPublishOrhestrator
{
    Task<List<PublishResult>> PublishPostToAll(PublishPostToAllRequest request);
}
