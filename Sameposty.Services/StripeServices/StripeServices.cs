using Sameposty.Services.Configurator;
using Sameposty.Services.Secrets;
using Sameposty.Services.Stripe;
using Stripe;
using Stripe.Checkout;


namespace Sameposty.Services.StripeServices;
public class StripeServices(ISecretsProvider secretsProvider, IConfigurator configurator) : IStripeService
{
    private readonly string StripeApiKey = secretsProvider.StripeApiKey;

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
                    Price = configurator.StripeSubscriptionPriceId,
                    Quantity = 1,
                },
            ],
            Discounts =
            [
                new SessionDiscountOptions()
                {
                    Coupon = "IDrizwBX",
                }
            ]
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
                new SubscriptionItemOptions() { Price = configurator.StripeSubscriptionPriceId },
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

    public Task<Session> CreatePortalSession(string stripeCustomerId)
    {
        throw new NotImplementedException();
    }
}
