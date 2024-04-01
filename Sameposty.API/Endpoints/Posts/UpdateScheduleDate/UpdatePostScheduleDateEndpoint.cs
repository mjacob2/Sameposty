using FastEndpoints;
using Hangfire;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.PostsPublishers.Orhestrator;

namespace Sameposty.API.Endpoints.Posts.UpdateScheduleDate;

public class UpdatePostScheduleDateEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor, IPostPublishOrchestrator postPublisher, IConfigurator configurator) : Endpoint<UpdatePostSheduledDateRequest>
{
    public override void Configure()
    {
        Patch("posts/updateScheduleDate");
    }

    public override async Task HandleAsync(UpdatePostSheduledDateRequest req, CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var getUserFromDbQuery = new GetUserByIdQuery(loggedUserId);
        var userFromDb = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        var getPostFromDbQuery = new GetPostByIdQuery() { PostId = req.PostId };
        var postFromDb = await queryExecutor.ExecuteQuery(getPostFromDbQuery);

        if (postFromDb.UserId != loggedUserId)
        {
            ThrowError("Brak uprawnień");
        }

        BackgroundJob.Delete(postFromDb.JobPublishId);

        DateTimeOffset cetDateTimeOffset = new(req.Date, TimeSpan.FromHours(1)); // Assuming req.Date is in CET (Central European Time)

        var request = new PublishPostToAllRequest()
        {
            BaseApiUrl = configurator.ApiBaseUrl,
            Post = postFromDb,
            Connections = new()
            {
                FacebookConnection = userFromDb.FacebookConnection,
                InstagramConnection = userFromDb.InstagramConnection,
            },
        };

        postFromDb.JobPublishId = BackgroundJob.Schedule(() => postPublisher.PublishPostToAll(request), cetDateTimeOffset);

        var updateScheduleDateCommand = new UpdatePostScheduleDateCommand(req.PostId, req.Date);

        var updatedPost = await commandExecutor.ExecuteCommand(updateScheduleDateCommand);

        await SendOkAsync(updatedPost, ct);
    }
}
