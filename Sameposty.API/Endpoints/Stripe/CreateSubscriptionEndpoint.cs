using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Stripe;
using Sameposty.Services.SubscriptionManager;

namespace Sameposty.API.Endpoints.Stripe;

public class CreateSubscriptionEndpoint(IQueryExecutor queryExecutor, IStripeService stripeService, ISubscriptionManager subscriptionManager) : Endpoint<CreateSubscriptionRequest>
{
    public override void Configure()
    {
        Post("addSubscription");
    }

    public override async Task HandleAsync(CreateSubscriptionRequest req, CancellationToken ct)
    {

        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(loggedUserId));

        try
        {
            await subscriptionManager.ManageSubscriptionCreated(userFromDb, req.CardTokenId);


            await SendOkAsync(userFromDb.Subscription, ct);
        }
        catch (Exception ex)
        {
            ThrowError(ex.Message);
        }
    }
}