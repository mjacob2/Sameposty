using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsPublishers.FacebookPublisher;

namespace Sameposty.Services.PostsPublishers.PostsPublisher;
public class PostsPublisher(IFacebookPostsPublisher facebookPublisher) : IPostsPublisher
{
    public async Task<List<PublishResult>> PublishPost(Post post, List<SocialMediaConnection> connections)
    {
        var results = new List<PublishResult>();

        foreach (var connection in connections)
        {
            if (connection.Platform == SocialMediaPlatform.Facebook)
            {
                var result = await facebookPublisher.PublishPost(post, connection);

                results.Add(result);
            }
        }

        return results;
    }
}
