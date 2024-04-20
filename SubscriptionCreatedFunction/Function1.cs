using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace SubscriptionCreatedFunction;

public class Function1(IQueryExecutor queryExecutor)
{
    private readonly IQueryExecutor _queryExecutor = queryExecutor;

    [Function("Function1")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        var user = await _queryExecutor.ExecuteQuery(new GetUserByIdQuery(1));
        return new OkObjectResult(user);
    }
}
