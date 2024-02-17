using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;

namespace Sameposty.API.Endpoints.Posts.UpdatePostDescription;

public class UpdatePostDescriptionEndpoint(ICommandExecutor commandExecutor) : Endpoint<UpdatePostDescriptionRequest>
{
    public override void Configure()
    {
        Patch("posts/updateDescriptionn");
    }

    public override async Task HandleAsync(UpdatePostDescriptionRequest req, CancellationToken ct)
    {
        var updateDescriptionCommand = new UpdatePostDescriptionCommand(req.PostId, req.PostDescription);

        var updatedPost = await commandExecutor.ExecuteCommand(updateDescriptionCommand);

        await SendOkAsync(updatedPost, ct);
    }
}
