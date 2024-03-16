using FastEndpoints;
using Sameposty.API.Endpoints.Posts.PostNow;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.SocialMediaConnections;
using Sameposty.Services.PostsPublishers.Orhestrator;

namespace Sameposty.API.Endpoints.Posts.PublishPostNow;

public class PublishPostNowEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IPostPublishOrhestrator postPublisher) : Endpoint<PublishPostNowRequest>
{


    public override void Configure()
    {
        Post("postNow");
    }

    public override async Task HandleAsync(PublishPostNowRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var getPostQuery = new GetPostByIdQuery() { PostId = req.PostId };
        var post = await queryExecutor.ExecuteQuery(getPostQuery);

        if (string.IsNullOrEmpty(post.ImageUrl))
        {
            ThrowError("Post nie ma zdjęcia!");
        }

        if (string.IsNullOrEmpty(post.Description))
        {
            ThrowError("Post nie ma opisu!");
        }

        var facebookConnectionQuery = new GetSocialMediaConnectionsByUserId() { UserId = userId, Platform = SocialMediaPlatform.Facebook };
        var connections = await queryExecutor.ExecuteQuery(facebookConnectionQuery);

        if (connections.Count == 0)
        {
            ThrowError("Najpierw połącz się z jakąś platformą social media!");
        }

        var results = new List<PublishResult>();

        try
        {
            results = await postPublisher.PublishPostToAll(post, connections);
        }
        catch (Exception ex)
        {
            ThrowError(ex.Message);
        }


        await SendOkAsync(results, ct);
    }
}
