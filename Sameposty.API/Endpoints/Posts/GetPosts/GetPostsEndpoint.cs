using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;

namespace Sameposty.API.Endpoints.Posts.GetPosts;

public class GetPostsEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("posts");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;

        var loggedUserIdInt = int.Parse(loggedUserId);

        var query = new GetPostsQuery(loggedUserIdInt);

        var posts = await queryExecutor.ExecuteQuery(query);

        var response = new GetPostsResponse(posts);

        await SendOkAsync(posts, ct);
    }
}
