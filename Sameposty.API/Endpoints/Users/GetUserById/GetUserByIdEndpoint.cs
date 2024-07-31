using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.GetUserById;

public class GetUserByIdEndpoint(IQueryExecutor queryExecutor) : Endpoint<GetUserByIdRequest, GetUserByIdResponse>
{
    public override void Configure()
    {
        Get("users/{UserId}");
        Policies("AdminOnly");
    }

    public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
    {
        var getUserFromDbQuery = new GetUserByIdWithAllPostsQuery(req.UserId);
        var user = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        user.Password = string.Empty;
        user.Salt = string.Empty;

        var response = new GetUserByIdResponse
        {
            User = user
        };

        await SendOkAsync(response, ct);
    }
}
