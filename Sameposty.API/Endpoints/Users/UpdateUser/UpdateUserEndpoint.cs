using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.UpdateUser;

public class UpdateUserEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor) : Endpoint<UpdateUserRequest>
{
    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("users");
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var getUserFromDbQuery = new GetUserByIdQuery(userId);
        var user = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        if (user == null)
        {
            ThrowError("Nie znaleziono użytkownika");
        }

        user.Name = req.Name;
        user.NIP = req.NIP;
        user.Street = req.Street;
        user.PostCode = req.PostCode;
        user.City = req.City;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });

        await SendOkAsync("updated", ct);
    }
}
