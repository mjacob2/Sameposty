using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.PostsPublisher;
public interface IPostsPublisher
{
    Task<List<PublishResult>> PublishPost(Post post, ConnectionsModel connections);
}
