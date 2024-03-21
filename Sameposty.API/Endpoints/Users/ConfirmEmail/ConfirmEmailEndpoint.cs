using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.ConfirmEmail;

public class ConfirmEmailEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("confirm");
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var getUserFromDbQuery = new GetUserByIdQuery(userId);
        var user = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        user.IsVerified = true;

        var updateUserCommand = new UpdateUserCommand() { Parameter = user };
        await commandExecutor.ExecuteCommand(updateUserCommand);

        await SendAsync(new
        {
            Id = user.Id,
            Username = user.Email,
        }, cancellation: ct);

    }
}
