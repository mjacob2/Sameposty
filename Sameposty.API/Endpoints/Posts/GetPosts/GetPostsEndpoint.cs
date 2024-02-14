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
        int articleID = Query<int>("t");

        var user = User;

        var claims = User.Claims.ToArray();

        var query = new GetPostsQuery();
        var posts = await queryExecutor.ExecuteQuery(query);

        var response = new GetPostsResponse(posts);

        await SendOkAsync(posts, ct);
    }
}
