using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Posts.UpdatePostDescription;

public class UpdatePostDescriptionEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : Endpoint<UpdatePostDescriptionRequest>
{
    public override void Configure()
    {
        Patch("posts/updateDescription");
    }

    public override async Task HandleAsync(UpdatePostDescriptionRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);

        var getUserFromDbQuery = new GetUserByIdQuery(id);
        var userFromDb = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        if (userFromDb.Id != id)
        {
            ThrowError("Brak uprawnień");
        }

        var updateDescriptionCommand = new UpdatePostDescriptionCommand(req.PostId, req.PostDescription);

        var updatedPost = await commandExecutor.ExecuteCommand(updateDescriptionCommand);

        await SendOkAsync(updatedPost, ct);
    }
}
