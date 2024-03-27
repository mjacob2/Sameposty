using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;

namespace Sameposty.API.Endpoints.Users.UpdateNip;

public class UpdateNipEndpoint(ICommandExecutor commandExecutor) : Endpoint<UpdateNipRequest>
{
    public override void Configure()
    {
        Patch("user/updateNIP");
    }

    public override async Task HandleAsync(UpdateNipRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);

        var updateNipCommand = new UpdateUserNipCommand(id, req.Nip);
        var updatedUser = await commandExecutor.ExecuteCommand(updateNipCommand);

        await SendOkAsync("updated", ct);
    }
}
