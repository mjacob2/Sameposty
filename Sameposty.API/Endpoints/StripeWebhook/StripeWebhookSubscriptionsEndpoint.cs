using FastEndpoints;
using Hangfire;
using Sameposty.Services.StripeWebhooksManagers.Subscriptions;
using Stripe;

namespace Sameposty.API.Endpoints.StripeWebhook;

public class StripeWebhookSubscriptionsEndpoint(IStripeSubscriptionWebhooksManager manager) : EndpointWithoutRequest
{
    private const string EndpointSecret = "whsec_ztLTSBpfRULHnqKam4NUHWyzuSYShLoA";

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
            BackgroundJob.Schedule(() =>
           manager.ManageSubscriptionCreated(subscription), TimeSpan.FromSeconds(10));
        }
        else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
        {
            BackgroundJob.Schedule(() =>
            manager.ManageSubscriptionDeleted(subscription), TimeSpan.FromSeconds(10));
        }
    }
}
