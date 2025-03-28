﻿using FastEndpoints;
using Sameposty.DataAccess.Commands.FacebookConnections;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.FacebookConnections;

namespace Sameposty.API.Endpoints.FacebookConnections.Delete;

public class DeleteFacebookConnectionEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("facebookConnection");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var facebookConnectionToDelete = await queryExecutor.ExecuteQuery(new GetFacebookConnectionByUserIdQuery(userId));

        if (facebookConnectionToDelete.UserId != userId)
        {
            ThrowError("Nie możesz tego usunąć!");
        }

        await commandExecutor.ExecuteCommand(new DeleteFacebookConnectionCommand() { Parameter = facebookConnectionToDelete });

        await SendOkAsync("deleted", ct);
    }
}
