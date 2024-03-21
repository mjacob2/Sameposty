using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.UpdateUserCompanyDescription;

public class UpdateUserCompanyDescriptionEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor) : Endpoint<UpdateUserCompanyDescriptionRequest>
{
    public override void Configure()
    {
        Patch("user/updateUserCompanyDescription");
    }

    public override async Task HandleAsync(UpdateUserCompanyDescriptionRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;

        var id = int.Parse(loggedUserId);

        var getUserFromDbQuery = new GetUserByIdQuery(id);
        var user = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        if (user == null)
        {
            ThrowError($"User with id {id} does not exists");
        }

        user.CompanyDescription = req.CompanyDescription;

        var updateUserCommand = new UpdateUserCommand() { Parameter = user };
        await commandExecutor.ExecuteCommand(updateUserCommand);

        await SendOkAsync(user, ct);
    }
}
