using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;

namespace Sameposty.API.Endpoints.Posts.UpdateIsApproved;

public class UpdateIsApprovedEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : Endpoint<UpdateIsApprovedRequest>
{
    public override void Configure()
    {
        Patch("posts/updateIsApproved");
    }

    public override async Task HandleAsync(UpdateIsApprovedRequest req, CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);

        var postFromDb = await queryExecutor.ExecuteQuery(new GetPostByIdQuery() { PostId = req.PostId });

        if (postFromDb.UserId != loggedUserId)
        {
            ThrowError("Brak uprawnień");
        }

        postFromDb.IsApproved = req.IsApproved;

        var updatedPost = await commandExecutor.ExecuteCommand(new UpdatePostCommand() { Parameter = postFromDb });

        await SendOkAsync(updatedPost, ct);
    }
}