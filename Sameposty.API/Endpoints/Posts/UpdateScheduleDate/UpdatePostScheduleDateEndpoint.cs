using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Posts.UpdateScheduleDate;

public class UpdatePostScheduleDateEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : Endpoint<UpdatePostSheduledDateRequest>
{
    public override void Configure()
    {
        Patch("posts/updateScheduleDate");
    }

    public override async Task HandleAsync(UpdatePostSheduledDateRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);

        var getUserFromDbQuery = new GetUserByIdQuery() { Id = id };
        var userFromDb = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        if (userFromDb.Id != id)
        {
            ThrowError("Brak uprawnień");
        }

        var updateScheduleDateCommand = new UpdatePostScheduleDateCommand(req.PostId, req.Date);

        var updatedPost = await commandExecutor.ExecuteCommand(updateScheduleDateCommand);

        await SendOkAsync(updatedPost, ct);
    }
}
