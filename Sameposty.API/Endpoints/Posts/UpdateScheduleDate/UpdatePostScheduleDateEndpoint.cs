using System.Text.Json;
using FastEndpoints;
using Hangfire;
using Newtonsoft.Json;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.PostsPublishers;

namespace Sameposty.API.Endpoints.Posts.UpdateScheduleDate;

public class UpdatePostScheduleDateEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor, IPostPublisher postPublisher) : Endpoint<UpdatePostSheduledDateRequest>
{
    public override void Configure()
    {
        Patch("posts/updateScheduleDate");
    }

    public override async Task HandleAsync(UpdatePostSheduledDateRequest req, CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var getUserFromDbQuery = new GetUserByIdQuery() { Id = loggedUserId };
        var userFromDb = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        var getPostFromDbQuery = new GetPostByIdQuery() { PostId = req.PostId };
        var postFromDb = await queryExecutor.ExecuteQuery(getPostFromDbQuery);

        if (postFromDb.UserId != loggedUserId)
        {
            ThrowError("Brak uprawnień");
        }

        BackgroundJob.Delete(postFromDb.JobPublishId);

        postFromDb.JobPublishId = BackgroundJob.Schedule(() => postPublisher.PublishPostToAll(postFromDb, userFromDb.SocialMediaConnections), req.Date);

        var updateScheduleDateCommand = new UpdatePostScheduleDateCommand(req.PostId, req.Date);

        var updatedPost = await commandExecutor.ExecuteCommand(updateScheduleDateCommand);

        await SendOkAsync(updatedPost, ct);
    }
}
