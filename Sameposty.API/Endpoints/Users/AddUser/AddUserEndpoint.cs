using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;

namespace Sameposty.API.Endpoints.Users.AddUser;

public class AddUserEndpoint(ICommandExecutor commandExecutor) : Endpoint<AddUserRequest, AddUserResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("addUser");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddUserRequest req, CancellationToken ct)
    {
        var user = new User()
        {
            CreatedDate = DateTime.UtcNow,
            Email = req.Email,
            Password = req.Password,
        };

        var command = new AddUserCommand() { Parameter = user };

        var newUserFromDb = await commandExecutor.ExecuteCommand(command);

        var response = new AddUserResponse()
        {
            NewUserId = newUserFromDb.Id,
        };

        await SendOkAsync(response, ct);
    }
}
