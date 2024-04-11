namespace Sameposty.Services.Stripe;
public interface IStripeService
{
    Task<string> CreateStripeCustomerCustomer(CreateStripeCustomerRequest req);
    Task<string> CreateSubscription(string stripeCustomerId);
}
