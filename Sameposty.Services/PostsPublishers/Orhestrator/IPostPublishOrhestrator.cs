using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.Orhestrator;
public interface IPostPublishOrhestrator
{
    Task<List<PublishResult>> PublishPostToAll(Post post, List<SocialMediaConnection> connections, string baseApiUrl);
}
