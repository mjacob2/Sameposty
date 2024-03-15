using Hangfire;
using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers;
public class PostPublisher : IPostPublisher
{
    [AutomaticRetry(Attempts = 0)]
    public Task PublishPostToAll(Post post, List<SocialMediaConnection> connections)
    {
        throw new NotImplementedException();

    }
}
