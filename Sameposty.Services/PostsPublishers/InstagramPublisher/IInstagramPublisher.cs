using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.InstagramPublisher;
public interface IInstagramPublisher
{
    public Task<PublishResult> PublishPost(Post post, InstagramConnection connection);
}
