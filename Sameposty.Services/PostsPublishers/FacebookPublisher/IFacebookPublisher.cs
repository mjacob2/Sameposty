using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.FacebookPublisher;
public interface IFacebookPublisher
{
    public Task<PublishResult> PublishPost(Post post, FacebookConnection connection);
}
