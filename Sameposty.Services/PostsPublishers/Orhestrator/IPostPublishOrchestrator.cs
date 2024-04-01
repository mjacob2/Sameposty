using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.Orhestrator;
public interface IPostPublishOrchestrator
{
    Task<List<PublishResult>> PublishPostToAll(PublishPostToAllRequest request);
}
