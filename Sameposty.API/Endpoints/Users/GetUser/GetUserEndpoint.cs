using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.GetUser;

public class GetUserEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("user");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;

        var id = int.Parse(loggedUserId);

        var getUserFromDbQuery = new GetUserByIdQuery() { Id = id };
        var user = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        await SendOkAsync(user, ct);
    }
}
