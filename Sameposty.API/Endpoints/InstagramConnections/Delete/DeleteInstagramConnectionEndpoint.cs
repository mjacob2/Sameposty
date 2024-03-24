using FastEndpoints;
using Sameposty.DataAccess.Commands.InstagramConenctions;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.InstagramConnections;

namespace Sameposty.API.Endpoints.InstagramConnections.Delete;

public class DeleteInstagramConnectionEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("instagramConnection");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var instagramConnectiontoDelete = await queryExecutor
            .ExecuteQuery(new GetInstagramConnectionByUserIdQuery(userId));

        if (instagramConnectiontoDelete.UserId != userId)
        {
            ThrowError("Nie możesz tego usunąć!");
        }

        await commandExecutor
            .ExecuteCommand(new DeleteInstagramConnectionCommand() { Parameter = instagramConnectiontoDelete });

        await SendOkAsync("deleted", ct);
    }
}
