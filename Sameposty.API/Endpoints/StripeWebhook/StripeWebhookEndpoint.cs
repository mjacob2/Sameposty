using FastEndpoints;
using Stripe;

namespace Sameposty.API.Endpoints.StripeWebhook;

public class StripeWebhookEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("stripe");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        const string endpointSecret = "whsec_60d96b3c95182480e4e40fbc985d3072db06d5a1f76c36ba353a7f4fa72718bd";

        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(ct);

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                "Stripe-Signature", endpointSecret);

            // Handle the event
            if (stripeEvent.Type == Events.SubscriptionScheduleAborted)
            {
            }
            else if (stripeEvent.Type == Events.SubscriptionScheduleCanceled)
            {
            }
            else if (stripeEvent.Type == Events.SubscriptionScheduleCompleted)
            {
            }
            else if (stripeEvent.Type == Events.SubscriptionScheduleCreated)
            {
            }
            else if (stripeEvent.Type == Events.SubscriptionScheduleReleased)
            {
            }
            else if (stripeEvent.Type == Events.SubscriptionScheduleUpdated)
            {
            }
            // ... handle other event types
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }


        }
        catch (StripeException e)
        {
            Console.Write(e.ToString());
        }
    }
}
