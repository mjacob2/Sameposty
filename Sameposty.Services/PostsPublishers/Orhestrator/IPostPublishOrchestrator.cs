using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;

namespace Sameposty.Services.PostsPublishers.Orhestrator;
public interface IPostPublishOrchestrator
{
    Task<List<PublishResult>> PublishPostToAll(PublishPostToAllRequest request);
}
