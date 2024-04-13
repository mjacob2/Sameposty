using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.FacebookConnections;
using Sameposty.DataAccess.Queries.InstagramConnections;
using Sameposty.Services.PostsPublishers.FacebookPublisher;
using Sameposty.Services.PostsPublishers.InstagramPublisher;

namespace Sameposty.Services.PostsPublishers.PostsPublisher;
public class PostsPublisher(IFacebookPublisher facebookPublisher, IInstagramPublisher instagramPublisher, IQueryExecutor queryExecutor) : IPostsPublisher
{
    public async Task<List<PublishResult>> PublishPost(Post post)
    {
        var results = new List<PublishResult>();

        if (!post.IsApproved)
        {
            return results;
        }

        var facebookConnection = await GetFacebookConnection(post.UserId);
        if (facebookConnection != null)
        {
            var result = await facebookPublisher.PublishPost(post, facebookConnection);
            results.Add(result);
        }

        var instagramConnection = await GetInstagramConnection(post.UserId);
        if (instagramConnection != null)
        {
            var result = await instagramPublisher.PublishPost(post, instagramConnection);
            results.Add(result);
        }

        return results;
    }

    private async Task<FacebookConnection> GetFacebookConnection(int userId)
    {
        var query = new GetFacebookConnectionByUserIdQuery(userId);
        var facebookConnection = await queryExecutor.ExecuteQuery(query);
        return facebookConnection;
    }

    private async Task<InstagramConnection> GetInstagramConnection(int userId)
    {
        var query = new GetInstagramConnectionByUserIdQuery(userId);
        var instagramConnection = await queryExecutor.ExecuteQuery(query);
        return instagramConnection;
    }
}
