using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.BasicInformations;

namespace Sameposty.API.Endpoints.BasicInformations.GetByUserId;

public class GetBasicInformationByUserIdEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("companyInformations");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);

        var getUserFromDbQuery = new GetBasicInformationByUserIdQuery() { UserId = id };
        var basicInformationFromDb = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        if (basicInformationFromDb == null)
        {
            ThrowError("Brak informacji o firmie");
        }

        await SendOkAsync(basicInformationFromDb, ct);
    }
}
