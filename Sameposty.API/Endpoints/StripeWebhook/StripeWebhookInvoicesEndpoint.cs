using FastEndpoints;
using Hangfire;
using Sameposty.Services.StripeWebhooksManagers;
using Stripe;
using Invoice = Stripe.Invoice;

namespace Sameposty.API.Endpoints.StripeWebhook;

public class StripeWebhookInvoicesEndpoint(IStripeWebhooksManager manager) : EndpointWithoutRequest
{

    private const string EndpointSecret = "whsec_I9nz7awy44YaT2tFnKMhIEniHvy9sz7y";

    public override void Configure()
    {
        Post("stripeWebhookInvoices");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(ct);

        var request = HttpContext.Request;

        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(ct);
        var stripeEvent = EventUtility.ConstructEvent(json,
                    request.Headers["Stripe-Signature"], EndpointSecret);

        if (stripeEvent.Data.Object is not Invoice invoice)
        {
            throw new ArgumentException("Invoice is null");
        }

        var userEmail = invoice.CustomerEmail;

        if (stripeEvent.Type == Events.InvoicePaid)
        {
            BackgroundJob.Schedule(() =>
            manager.ManageInvoicePaid(userEmail), TimeSpan.FromSeconds(2));


        }
        else if (stripeEvent.Type == Events.InvoicePaymentFailed)
        {
            BackgroundJob.Schedule(() =>
            manager.ManageInvoicePaymentFailed(userEmail), TimeSpan.FromSeconds(2));
        }
        else
        {
            Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
        }
    }

    public static string GetStreetNameWithNumbers(string street, string buildingNumber, string? flatNumber)
    {
        var respone = street + " " + buildingNumber;

        if (!string.IsNullOrEmpty(flatNumber))
        {
            respone += $"/{flatNumber}";
        }

        return respone;
    }
}
