using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Stripe;
using Sameposty.Services.SubscriptionManager;

namespace Sameposty.API.Endpoints.Stripe.SubscriptionCheckoutSession;

public class CreateSubscriptionCheckoutSessionEndpoint(IQueryExecutor queryExecutor, IStripeService stripe, ISubscriptionManager manager) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("create-checkout-session");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        ThrowError("ale jakiś niemiły błą");
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(loggedUserId));

        var stripeCustomer = await manager.CreateStripeCustomer(userFromDb);

        var session = await stripe.CreateSubscriptionSession(stripeCustomer, userFromDb.Id);

        await SendOkAsync(session.Url, ct);
    }
}
