using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers.FacebookPublisher;
public interface IFacebookPostsPublisher
{
    public Task<PublishResult> PublishPost(Post post, SocialMediaConnection connection);
}
