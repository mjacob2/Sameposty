using FastEndpoints;
using Hangfire;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.ConfiguratorService;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;

namespace Sameposty.API.Endpoints.Posts.UpdateScheduleDate;

public class UpdatePostScheduleDateEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor, IPostPublishOrchestrator postPublisher, IConfigurator configurator) : Endpoint<UpdatePostSheduledDateRequest>
{
    public override void Configure()
    {
        Patch("posts/updateScheduleDate");
    }

    public override async Task HandleAsync(UpdatePostSheduledDateRequest req, CancellationToken ct)
    {
        var reqDateUtc = req.Date.ToUniversalTime();
        var utcNow = DateTime.UtcNow;

        if (reqDateUtc < utcNow)
        {
            ThrowError("Postu nie można zaplanować na wcześniej niż za 10 minut!");
        }

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

        if (postFromDb.IsPublishingInProgress || postFromDb.IsPublished)
        {
            ThrowError("Nie można już edytować tego posta");
        }

        BackgroundJob.Delete(postFromDb.JobPublishId);

        var request = new PublishPostToAllRequest()
        {
            BaseApiUrl = configurator.ApiBaseUrl,
            PostId = postFromDb.Id,
        };

        DateTimeOffset localDateTimeOffset = new(req.Date, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time").GetUtcOffset(req.Date));
        DateTimeOffset utcDateTimeOffset = localDateTimeOffset.UtcDateTime;

        postFromDb.JobPublishId = BackgroundJob.Schedule(() => postPublisher.PublishPostToAll(request), utcDateTimeOffset.UtcDateTime);

        postFromDb.ShedulePublishDate = req.Date;

        var updateScheduleDateCommand = new UpdatePostCommand() { Parameter = postFromDb };

        var updatedPost = await commandExecutor.ExecuteCommand(updateScheduleDateCommand);

        await SendOkAsync(updatedPost, ct);
    }
}
