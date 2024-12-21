using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.ConfiguratorService;
using Sameposty.Services.FileRemoverService;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.API.Endpoints.Posts.RegenerateImage;

public class RegenerateImageEndpoint(IQueryExecutor queryExecutor, IImageGeneratingOrchestrator imageOrchestrator, IConfigurator configurator, ICommandExecutor commandExecutor, IFileRemover fileRemover, ITextGenerator textGenerator) : Endpoint<RegenerateImageRequest>
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

        if (userFromDb.ImageTokensLeft < 1)
        {
            ThrowError("Brak wystarczającej ilości tokenów do generowania obrazów!");
        }
        var prompt = string.Empty;

        prompt = req.GeneratePrompt
            ? await textGenerator.GeneratePromptForImageForPost(userFromDb.BasicInformation.ProductsAndServices)
            : req.Prompt;

        if (!req.GeneratePrompt && string.IsNullOrEmpty(req.Prompt))
        {
            ThrowError("Polecenie nie może być puste!");
        }

        var newImageName = await imageOrchestrator.GenerateImageFromUserPrompt(prompt);

        var imageUrl = $"{configurator.ApiBaseUrl}/{newImageName}";

        var postToUpdate = await queryExecutor.ExecuteQuery(new GetPostByIdQuery() { PostId = req.PostId });

        if (postToUpdate.IsPublishingInProgress || postToUpdate.IsPublished)
        {
            ThrowError("Nie można już edytować tego posta");
        }

        fileRemover.RemovePostImage(postToUpdate.ImageUrl);

        postToUpdate.ImageUrl = imageUrl;

        var updatePostCommand = new UpdatePostCommand() { Parameter = postToUpdate };

        await commandExecutor.ExecuteCommand(updatePostCommand);

        userFromDb.DecreaseImageTokens();

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });

        await SendOkAsync(imageUrl, ct);
    }
}
