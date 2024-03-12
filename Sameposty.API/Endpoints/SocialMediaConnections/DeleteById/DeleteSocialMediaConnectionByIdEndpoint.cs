using FastEndpoints;
using Sameposty.DataAccess.Commands.SocialMediaConnectors;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.SocialMediaConnections;

namespace Sameposty.API.Endpoints.SocialMediaConnections.DeleteById;

public class DeleteSocialMediaConnectionByIdEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor) : Endpoint<DeleteSocialMediaConnectionByIdRequest>
{
    public override void Configure()
    {
        Delete("socialMediaConections");
    }

    public override async Task HandleAsync(DeleteSocialMediaConnectionByIdRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var getConnectionCommand = new GetSocialMediaConnectionByIdQuery() { SocialMediaConnectionId = req.Id };
        var socialMediaConnectionToDelete = await queryExecutor.ExecuteQuery(getConnectionCommand);

        if(socialMediaConnectionToDelete.UserId != userId)
        {
            ThrowError("Nie możesz tego usunąć!");
        }

        var deleteCommand = new DeleteSocialMediaConnectionCommand() { Parameter = socialMediaConnectionToDelete };
        var deletedConnection = await commandExecutor.ExecuteCommand(deleteCommand);

        await SendOkAsync(deletedConnection, ct);
    }
}
