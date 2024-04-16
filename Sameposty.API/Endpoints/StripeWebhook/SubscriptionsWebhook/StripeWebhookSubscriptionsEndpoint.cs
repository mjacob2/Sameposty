using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.EmailService;
using Sameposty.Services.SubscriptionManager;
using Stripe;

namespace Sameposty.API.Endpoints.StripeWebhook.SubscriptionsWebhook;

public class StripeWebhookSubscriptionsEndpoint(ISubscriptionManager subscriptionManager, IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IEmailService email) : EndpointWithoutRequest
{
    private const string EndpointSecret = "whsec_KnEqHAw3orJ4oihkjT08kVYL6rUmCCsn";

    public override void Configure()
    {
        Post("stripeWebhookSubscriptions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(ct);

        var request = HttpContext.Request;

        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(ct);
        var stripeEvent = EventUtility.ConstructEvent(json,
                    request.Headers["Stripe-Signature"], EndpointSecret);

        if (stripeEvent.Data.Object is not Subscription subscription)
        {
            throw new ArgumentException("Subscription is null");
        }

        if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
        {
            var userId = GetUserIdFromStripeSubscriptionEvent(subscription);
            var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(userId));

            userFromDb.Subscription.StipeSubscriptionId = subscription.Id;
            userFromDb.Subscription.SubscriptionCurrentPeriodStart = subscription.CurrentPeriodStart.ToString();
            userFromDb.Subscription.SubscriptionCurrentPeriodEnd = subscription.CurrentPeriodEnd.ToString();
            // TODO : Add amount paid to subscription
            await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
            await email.EmailUserSubscriptionCreated(userFromDb.Email);
        }
        else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
        {
            var userId = GetUserIdFromStripeSubscriptionEvent(subscription);
            var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(userId));
            userFromDb.Subscription.StipeSubscriptionId = null;

            await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
            await email.EmailUserSubscriptionDeleted(userFromDb.Email);
        }
    }

    private static int GetUserIdFromStripeSubscriptionEvent(Subscription subscription)
    {
        var userId = subscription.Metadata["userId"];
        return string.IsNullOrEmpty(userId) ? throw new ArgumentException("userId is missing in stripe metadata") : int.Parse(userId);
    }
}
