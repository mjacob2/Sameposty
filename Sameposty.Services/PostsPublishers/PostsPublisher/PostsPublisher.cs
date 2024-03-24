using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsPublishers.FacebookPublisher;
using Sameposty.Services.PostsPublishers.InstagramPublisher;

namespace Sameposty.Services.PostsPublishers.PostsPublisher;
public class PostsPublisher(IFacebookPublisher facebookPublisher, IInstagramPublisher instagramPublisher) : IPostsPublisher
{
    public async Task<List<PublishResult>> PublishPost(Post post, ConnectionsModel connections)
    {
        var results = new List<PublishResult>();

        if (connections.FacebookConnection != null)
        {
            var result = await facebookPublisher.PublishPost(post, connections.FacebookConnection);
            results.Add(result);
        }

        if (connections.InstagramConnection != null)
        {
            var result = await instagramPublisher.PublishPost(post, connections.InstagramConnection); results.Add(result);
            results.Add(result);
        }



        return results;
    }
}
