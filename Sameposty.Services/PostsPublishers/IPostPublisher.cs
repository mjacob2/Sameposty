using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers;
public interface IPostPublisher
{
    Task PublishPostToAll(Post post, List<SocialMediaConnection> connections);
}
