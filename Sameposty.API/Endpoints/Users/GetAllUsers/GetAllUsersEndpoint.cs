using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.GetAllUsers;

public class GetAllUsersEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("users");
        Policies("AdminOnly");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var getUserFromDbQuery = new GetAllUsersQuery();
        var users = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        foreach (var user in users)
        {
            user.Password = string.Empty;
            user.Salt = string.Empty;
        }

        await SendOkAsync(users, ct);
    }
}
