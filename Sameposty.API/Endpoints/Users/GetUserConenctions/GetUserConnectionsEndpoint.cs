using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.GetUserConenctions;

public class GetUserConnectionsEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("socialMediaConnectionsByUserId");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;

        var loggedUserIdInt = int.Parse(loggedUserId);

        var query = new GetUserByIdWithConnectionsQuery(loggedUserIdInt);

        var user = await queryExecutor.ExecuteQuery(query);

        var response = new Response(user);

        await SendOkAsync(response, ct);
    }
}
