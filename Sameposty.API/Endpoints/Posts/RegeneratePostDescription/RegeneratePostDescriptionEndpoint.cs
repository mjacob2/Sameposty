using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.BasicInformations;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.API.Endpoints.Posts.RegeneratePostDescription;

public class RegeneratePostDescriptionEndpoint(ITextGenerator textGenerator, IQueryExecutor queryExecutor, ICommandExecutor commandExecutor) : Endpoint<RegeneratePostDescriptionRequest>
{
    public override void Configure()
    {
        Patch("posts/regenerateDescription");
    }
    public override async Task HandleAsync(RegeneratePostDescriptionRequest req, CancellationToken ct)
    {

        if (string.IsNullOrEmpty(req.Prompt))
        {
            ThrowError("Polecenie nie może być puste!");
        }

        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);

        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserOnlyByIdQuery(loggedUserId));

        if (userFromDb.TextTokensLeft < 1)
        {
            ThrowError("Brak wystarczającej ilości tokenów do generowania tekstów!");
        }

        var basicInformation = await queryExecutor.ExecuteQuery(new GetBasicInformationByUserIdQuery(loggedUserId));
        var postToUpdate = await queryExecutor.ExecuteQuery(new GetPostByIdQuery() { PostId = req.PostId });

        if (postToUpdate.IsPublishingInProgress || postToUpdate.IsPublished)
        {
            ThrowError("Nie można już edytować tego posta");
        }

        var regenerateRequest = new ReGeneratePostRequest()
        {
            UserPrompt = req.Prompt,
            Audience = basicInformation.Audience,
            BrandName = basicInformation.BrandName,
        };

        var newDescription = await textGenerator.ReGeneratePostDescription(regenerateRequest);

        postToUpdate.Description = newDescription;

        var updateDescriptionCommand = new UpdatePostCommand() { Parameter = postToUpdate };

        await commandExecutor.ExecuteCommand(updateDescriptionCommand);

        userFromDb.DecreaseTextTokens();

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });

        await SendOkAsync(newDescription, ct);
    }
}
