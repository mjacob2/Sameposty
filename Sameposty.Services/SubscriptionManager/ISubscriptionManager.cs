using Sameposty.DataAccess.Entities;
using Stripe;

namespace Sameposty.Services.SubscriptionManager;
public interface ISubscriptionManager
{
    Task ManageSubscriptionCreated(User userFromDb, string cardTokenId);

    Task ManageSubscriptionCanceled(User userFromDb);

    Task<Customer> CreateStripeCustomer(User userFromDb);
}
