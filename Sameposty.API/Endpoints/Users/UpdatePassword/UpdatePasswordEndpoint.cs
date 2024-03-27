using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Hasher;

namespace Sameposty.API.Endpoints.Users.UpdatePassword;

public class UpdatePasswordEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor) : Endpoint<UpdatePasswordRequest>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("update-password");
    }
    public override async Task HandleAsync(UpdatePasswordRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Password))
        {
            ThrowError("Hasło nie może być puste");
        }
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var getUserFromDbQuery = new GetUserByIdQuery(userId);
        var user = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        var salt = Hasher.GetSalt();
        var passwordHashed = Hasher.HashPassword(req.Password, salt);

        user.Salt = salt;
        user.Password = passwordHashed;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });

        await SendAsync(new
        {
            Id = user.Id,
            Username = user.Email,
        }, cancellation: ct);
    }
}
