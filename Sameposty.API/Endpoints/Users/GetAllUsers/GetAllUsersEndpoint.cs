using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Mappings;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.GetAllUsers;

public class GetAllUsersEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest<GetAllUsersResponse>
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

        var dto = users.Select(u => u.MapToUserBasicInfo()).ToList();

        var response = new GetAllUsersResponse
        {
            Users = dto
        };

        await SendOkAsync(response, ct);
    }
}
