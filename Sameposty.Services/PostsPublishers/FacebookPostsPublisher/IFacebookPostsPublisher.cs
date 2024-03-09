using Sameposty.Services.PostsPublishers.FacebookPostsPublisher.Models;

namespace Sameposty.Services.PostsPublishers.FacebookPostsPublisher;
public interface IFacebookPostsPublisher
{
    public Task<string> PublishPost(FacegookPostToPublish post, FacebookPageInfo pageInfo);
}
