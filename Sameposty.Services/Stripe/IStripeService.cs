using Stripe;

namespace Sameposty.Services.Stripe;
public interface IStripeService
{
    Task<Customer> CreateStripeCustomerCustomer(CreateStripeCustomerRequest req);
    Task<Subscription> CreateSubscription(string stripeCustomerId, string userId);
    Task CancelSubscription(string subscriptionId);

    Task DeleteCard(string stripeCustomerId, string stripePaymentCardId);

    Task<Subscription> GetSubscription(string subscriptionId);

}
