using Sameposty.Services.Stripe;
using Stripe;
using Stripe.Checkout;

namespace Sameposty.Services.StripeServices;
public interface IStripeService
{
    Task<Session> CreatePortalSession(string stripeCustomerId);

    Task<Session> CreateSubscriptionSession(string stripeCustomerId, int userId);

    Task<Customer> CreateStripeCustomer(CreateStripeCustomerRequest req);

    Task<Subscription> CreateSubscription(string stripeCustomerId, string userId);

    Task CancelSubscription(string subscriptionId);

    Task<Subscription> GetSubscription(string subscriptionId);
}
