using FastEndpoints;
using Hangfire;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.Services.FileRemover;

namespace Sameposty.API.Endpoints.Posts.Delete;

public class DeletePostEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IFileRemover fileRemover) : Endpoint<DeletePostRequest>
{
    public override void Configure()
    {
        Delete("posts");
    }

    public override async Task HandleAsync(DeletePostRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var getPostCommand = new GetPostByIdQuery() { PostId = req.Id };
        var postToDelete = await queryExecutor.ExecuteQuery(getPostCommand);

        if (postToDelete.UserId != userId)
        {
            ThrowError("Nie możesz tego usunąć!");
        }
        BackgroundJob.Delete(postToDelete.JobPublishId);

        var deleteCommand = new DeletePostCommand() { Parameter = postToDelete };
        var deletedPost = await commandExecutor.ExecuteCommand(deleteCommand);

        fileRemover.RemovePostImage(postToDelete.ImageUrl);

        await SendOkAsync(deletedPost, ct);
    }
}
