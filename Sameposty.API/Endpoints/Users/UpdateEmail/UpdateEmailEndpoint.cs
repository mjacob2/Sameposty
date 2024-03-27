using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;

namespace Sameposty.API.Endpoints.Users.UpdateEmail;

public class UpdateEmailEndpoint(ICommandExecutor commandExecutor) : Endpoint<UpdateEmailRequest>
{
    public override void Configure()
    {
        Patch("user/updateEmail");
    }

    public override async Task HandleAsync(UpdateEmailRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);

        var updateEmailCommand = new UpdateUserEmailCommand(id, req.Email);
        var updatedUser = await commandExecutor.ExecuteCommand(updateEmailCommand);

        await SendOkAsync("updated", ct);
    }
}
