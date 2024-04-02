using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.FileRemover;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;

namespace Sameposty.API.Endpoints.Posts.RegenerateImage;

public class RegenerateImageEndpoint(IQueryExecutor queryExecutor, IImageGeneratingOrchestrator imageOrchestrator, IConfigurator configurator, ICommandExecutor commandExecutor, IFileRemover fileRemover) : Endpoint<RegenerateImageRequest>
{
    public override void Configure()
    {
        Patch("posts/regenerateImage");
    }

    public override async Task HandleAsync(RegenerateImageRequest req, CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);

        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserOnlyByIdQuery(loggedUserId));

        if (userFromDb.GetImageTokensLeft() < 1)
        {
            ThrowError("Brak wystarczającej ilości tokenów do generowania obrazów!");
        }

        var newImageName = await imageOrchestrator.GenerateImageFromUserPrompt(req.Prompt);

        var imageUrl = $"{configurator.ApiBaseUrl}/{newImageName}";

        var postToUpdate = await queryExecutor.ExecuteQuery(new GetPostByIdQuery() { PostId = req.PostId });

        fileRemover.RemovePostImage(postToUpdate.ImageUrl);

        postToUpdate.ImageUrl = imageUrl;

        var updatePostCommand = new UpdatePostCommand() { Parameter = postToUpdate };

        await commandExecutor.ExecuteCommand(updatePostCommand);

        userFromDb.ImageTokensUsed++;
        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });

        await SendOkAsync(imageUrl, ct);
    }
}
