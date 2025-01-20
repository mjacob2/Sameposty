using FastEndpoints;
using Hangfire;
using Sameposty.Services.SecretsService;
using Sameposty.Services.StripeWebhooksManagers.Subscriptions;
using Stripe;

namespace Sameposty.API.Endpoints.StripeWebhooks;

public class StripeWebhookSubscriptionsEndpoint(IStripeSubscriptionWebhooksManager manager, ISecretsProvider secrets) : EndpointWithoutRequest
{
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
                    request.Headers["Stripe-Signature"], secrets.StripeSubscriptionsWebhookKey);

        if (stripeEvent.Data.Object is not Subscription subscription)
        {
            throw new ArgumentException("Subscription is null");
        }

        if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
        {
            BackgroundJob.Schedule(() =>
           manager.ManageSubscriptionCreated(subscription), TimeSpan.FromSeconds(2));
        }
        else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
        {
            BackgroundJob.Schedule(() =>
            manager.ManageSubscriptionDeleted(subscription), TimeSpan.FromSeconds(2));
        }
    }
}
