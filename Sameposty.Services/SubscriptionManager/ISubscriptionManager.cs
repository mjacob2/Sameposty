using Sameposty.DataAccess.Entities;
using Stripe;

namespace Sameposty.Services.SubscriptionManager;
public interface ISubscriptionManager
{
    Task ManageSubscriptionCreated(User userFromDb);

    Task ManageSubscriptionCanceled(User userFromDb);

    Task<Customer> GetStripeCustomerId(User userFromDb);
}
