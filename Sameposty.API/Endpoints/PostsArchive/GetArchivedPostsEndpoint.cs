using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;

namespace Sameposty.API.Endpoints.PostsArchive;

public class GetArchivedPostsEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("archive");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);

        var archivedPostsquery = new GetArchivedPostsByUserId() { UserId = loggedUserId };
        var archivedPosts = await queryExecutor.ExecuteQuery(archivedPostsquery);

        await SendOkAsync(archivedPosts, ct);
    }
}
