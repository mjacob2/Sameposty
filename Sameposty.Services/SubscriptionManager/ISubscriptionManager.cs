using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.SubscriptionManager;
public interface ISubscriptionManager
{
    Task ManageSubscriptionCreated(User userFromDb, string cardTokenId);

    Task ManageSubscriptionCanceled(User userFromDb);
}
