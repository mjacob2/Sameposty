using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsPublishers.FacebookPublisher;

namespace Sameposty.Services.PostsPublishers.PostsPublisher;
public class PostsPublisher(IFacebookPostsPublisher facebookPublisher) : IPostsPublisher
{
    public async Task<List<PublishResult>> PublishPost(Post post, ConnectionsModel connections)
    {
        var results = new List<PublishResult>();

        var result = await facebookPublisher.PublishPost(post, connections.FacebookConnection);

        results.Add(result);


        return results;
    }
}
