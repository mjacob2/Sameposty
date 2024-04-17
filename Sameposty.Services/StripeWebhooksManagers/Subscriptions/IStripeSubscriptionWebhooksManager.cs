using Stripe;

namespace Sameposty.Services.StripeWebhooksManagers.Subscriptions;
public interface IStripeSubscriptionWebhooksManager
{
    Task ManageSubscriptionCreated(Subscription subscription);
    Task ManageSubscriptionDeleted(Subscription subscription);
}
