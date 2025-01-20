using FastEndpoints;
using Sameposty.API.Endpoints.Posts.PostNow;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services;
using Sameposty.Services.ConfiguratorService;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;

namespace Sameposty.API.Endpoints.Posts.PublishPostNow;

public class PublishPostNowEndpoint(IQueryExecutor queryExecutor, IPostPublishOrchestrator postPublisher, IConfigurator configurator) : Endpoint<PublishPostNowRequest>
{
    public override void Configure()
    {
        Post("postNow");
    }

    public override async Task HandleAsync(PublishPostNowRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdWithConnectionsQuery(userId));

        var getPostQuery = new GetPostByIdQuery() { PostId = req.PostId };
        var postToPublish = await queryExecutor.ExecuteQuery(getPostQuery);

        if (postToPublish.UserId != userId)
        {
            ThrowError("Ten post nie należy do Ciebie! Nie możesz go opublikować!");
        }

        if (!postToPublish.IsApproved)
        {
            ThrowError("Najpierw zatwierdź post!");
        }

        if (string.IsNullOrEmpty(postToPublish.ImageUrl))
        {
            ThrowError("Post nie ma zdjęcia!");
        }

        if (postToPublish.ImageUrl.Contains(AppConstants.DefaultImageName))
        {
            ThrowError("Post nie ma zdjęcia!");
        }

        if (string.IsNullOrEmpty(postToPublish.Description))
        {
            ThrowError("Post nie ma opisu!");
        }

        var results = new List<PublishResult>();

        try
        {
            var publishRequest = new PublishPostToAllRequest()
            {
                BaseApiUrl = configurator.ApiBaseUrl,
                PostId = postToPublish.Id,

            };
            results = await postPublisher.PublishPostToAll(publishRequest);
        }
        catch (Exception ex)
        {
            ThrowError(ex.Message);
        }


        await SendOkAsync(results, ct);
    }
}
