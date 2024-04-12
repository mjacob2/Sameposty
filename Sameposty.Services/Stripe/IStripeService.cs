using Stripe;

namespace Sameposty.Services.Stripe;
public interface IStripeService
{
    Task<Customer> CreateStripeCustomerCustomer(CreateStripeCustomerRequest req);
    Task<Subscription> CreateSubscription(string stripeCustomerId);
}
