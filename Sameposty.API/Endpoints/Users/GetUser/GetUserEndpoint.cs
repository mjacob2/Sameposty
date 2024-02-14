using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.GetUser;

public class GetUserEndpoint(IQueryExecutor queryExecutor) : Endpoint<GetUserRequest>
{
    public override void Configure()
    {
        Get("user");
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        var user = User;
        //TODO prevent from getting user id different than currently logged in in TOKEN!

        int id = Query<int>("t");

        //var id2 = int.Parse(id);

        var query = new GetUserByIdQuery() { Id = id };

        var userFromDb = await queryExecutor.ExecuteQuery(query);

        await SendOkAsync(userFromDb, ct);
    }
}
