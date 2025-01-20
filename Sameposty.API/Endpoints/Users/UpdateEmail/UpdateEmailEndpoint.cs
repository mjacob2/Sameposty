using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.UpdateEmail;

public class UpdateEmailEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : Endpoint<UpdateEmailRequest>
{
    public override void Configure()
    {
        Patch("user/updateEmail");
    }

    public override async Task HandleAsync(UpdateEmailRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);

        var userToUpdate = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(id));

        if (userToUpdate == null)
        {
            await SendNotFoundAsync(ct);
        }

        userToUpdate.Email = req.Email;

        var updatedUser = await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userToUpdate });

        await SendOkAsync("updated", ct);
    }
}
