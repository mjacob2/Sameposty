using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.SubscriptionManager;

namespace Sameposty.API.Endpoints.Stripe.CancelSubscription;

public class CancelSubscriptionEndpoint(IQueryExecutor queryExecutor, ISubscriptionManager subscriptionManager) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("cancelSubscription");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {

        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(loggedUserId));

        try
        {
            await subscriptionManager.ManageSubscriptionCanceled(userFromDb);
            await SendOkAsync(userFromDb.Subscription, ct);
        }
        catch (Exception ex)
        {
            ThrowError(ex.Message);
        }
    }
}
