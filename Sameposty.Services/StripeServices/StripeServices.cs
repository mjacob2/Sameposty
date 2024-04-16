using Sameposty.Services.Configurator;
using Sameposty.Services.Secrets;
using Sameposty.Services.Stripe;
using Stripe;
using Stripe.Checkout;


namespace Sameposty.Services.StripeServices;
public class StripeServices(ISecretsProvider secretsProvider, IConfigurator configurator) : IStripeService
{
    private const string Price = "price_1P3iW6LJdNESLWLI7IJLtxNq";

    private readonly string StripeApiKey = secretsProvider.StripeApiKey;

    private readonly string ReturnUrl = "https://sameposty.pl";


    public async Task<Session> CreateSubscriptionSession(string stripeCustomerId, int userId)
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var options = new SessionCreateOptions
        {
            SuccessUrl = configurator.SubscriptionSuccessPaymentUrl + "?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = configurator.SubscriptionFailedPaymentUrl,
            Mode = "subscription",
            Customer = stripeCustomerId,
            SubscriptionData = new SessionSubscriptionDataOptions
            {
                Metadata = new Dictionary<string, string> { { "userId", userId.ToString() } },
            },
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = Price,
                    Quantity = 1,
                },
            ],
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session;
    }

    public async Task<Subscription> CreateSubscription(string stripeCustomerId, string userId)
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var options = new SubscriptionCreateOptions
        {
            Metadata = new Dictionary<string, string> { { "userId", userId } },
            Customer = stripeCustomerId,
            Items =
            [
                new SubscriptionItemOptions() { Price = Price },
            ],
        };
        var service = new SubscriptionService();
        var result = await service.CreateAsync(options);
        return result;
    }

    public async Task<Customer> CreateStripeCustomer(CreateStripeCustomerRequest req)
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var options = new CustomerCreateOptions
        {
            Metadata = req.Metadata,
            Source = req.CardTokenId,
            Name = req.Name,
            Email = req.Email,
            Shipping = new ShippingOptions()
            {
                Address = new AddressOptions()
                {
                    Country = "Polska",
                    City = req.City,
                    Line1 = req.Street,
                    PostalCode = req.PostalCode
                },
                Name = req.Name,
            },
            TaxIdData = [new CustomerTaxIdDataOptions()
            {
                Type = "eu_vat",
                Value = "PL" + req.NIP,
            }
            ],

        };
        var service = new CustomerService();

        var resposne = await service.CreateAsync(options);

        return resposne;
    }

    public async Task UpdateCutomer()
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var options = new CustomerUpdateOptions
        {
            Metadata = new Dictionary<string, string> { { "order_id", "6735" } },
        };
        var service = new CustomerService();
        await service.UpdateAsync("cus_NffrFeUfNV2Hib", options);
    }

    public async Task CancelSubscription(string subscriptionId)
    {
        StripeConfiguration.ApiKey = StripeApiKey;
        var service = new SubscriptionService();
        await service.CancelAsync(subscriptionId);
    }

    public async Task<Subscription> GetSubscription(string subscriptionId)
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var service = new SubscriptionService();
        return await service.GetAsync(subscriptionId);

    }

    public async Task DeleteCard(string stripeCustomerId, string stripePaymentCardId)
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var service = new SourceService();
        await service.DetachAsync(stripeCustomerId, stripePaymentCardId);
    }

    public Task<Session> CreatePortalSession(string stripeCustomerId)
    {
        throw new NotImplementedException();
    }
}
