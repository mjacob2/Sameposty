using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.SecretsService;
using Stripe;
using Stripe.BillingPortal;

namespace Sameposty.API.Endpoints.Stripe.PortalSession;

public class CreatePortalSessionEndpoint(IQueryExecutor queryExecutor, ISecretsProvider secretsProvider) : EndpointWithoutRequest
{
    private readonly string StripeApiKey = secretsProvider.StripeApiKey;

    public override void Configure()
    {
        Get("create-portal-session");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(loggedUserId));

        StripeConfiguration.ApiKey = StripeApiKey;

        var options = new SessionCreateOptions
        {
            Customer = userFromDb.Subscription.StripeCustomerId,
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options, cancellationToken: ct);

        await SendOkAsync(session.Url, ct);
    }
}
