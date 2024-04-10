using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.EasyCart;
using Sameposty.Services.EmailService;

namespace Sameposty.API.Endpoints.EasyCartWebhook;

public class EasyCartWebhookEndpoint(IQueryExecutor queryExecutor, IEmailService email, IEasyCart easyCart) : Endpoint<EasyCartRequest>
{
    public override void Configure()
    {
        Post("easycartDelete");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EasyCartRequest req, CancellationToken ct)
    {
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByEmailQuery() { Email = "jakubicki.m@gmail.com" });

        if (userFromDb == null)
        {
            await email.SendNotifyInvalidSubscriptionEmail(req.CustomerEmail, req.CustomerId, req.OrderId);

            return;
        }

        if (req.Event == "subscription_created")
        {
            var model = new EasyCartSubscriptionModel()
            {
                AmountPaid = req.AmountPaid,
                CustomerEmail = req.CustomerEmail,
                OrderHasInvoice = req.OrderHasInvoice,
                OrderId = req.OrderId,
                SubscriptionCurrentPeriodEnd = req.SubscriptionCurrentPeriodEnd,
                SubscriptionCurrentPeriodStart = req.SubscriptionCurrentPeriodStart,
            };

            await easyCart.ActForSubcriptionCreated(model, userFromDb);
        }

        if (req.Event == "subscription_renewed")
        {
            if (userFromDb.Subscription == null)
            {
                throw new ArgumentNullException(nameof(userFromDb.Subscription));
            }
            var model = new EasyCartSubscriptionModel()
            {
                AmountPaid = req.AmountPaid,
                CustomerEmail = req.CustomerEmail,
                OrderHasInvoice = req.OrderHasInvoice,
                OrderId = req.OrderId,
                SubscriptionCurrentPeriodEnd = req.SubscriptionCurrentPeriodEnd,
                SubscriptionCurrentPeriodStart = req.SubscriptionCurrentPeriodStart,
            };

            await easyCart.ActForSubcriptionReneved(model, userFromDb);
        }

        if (req.Event == "subscription_canceled")
        {
            if (userFromDb.Subscription == null)
            {
                throw new ArgumentNullException(nameof(userFromDb.Subscription));
            }
            var model = new EasyCartSubscriptionModel()
            {
                AmountPaid = req.AmountPaid,
                CustomerEmail = req.CustomerEmail,
                OrderHasInvoice = req.OrderHasInvoice,
                OrderId = req.OrderId,
                SubscriptionCurrentPeriodEnd = req.SubscriptionCurrentPeriodEnd,
                SubscriptionCurrentPeriodStart = req.SubscriptionCurrentPeriodStart,
            };

            await easyCart.ActForSubcriptionCanceled(model, userFromDb);
        }

        await SendOkAsync(ct);
    }
}
