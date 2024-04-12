using Sameposty.DataAccess.Entities;
using Sameposty.Services.EasyCart;

namespace Sameposty.Services.SubscriptionManager;
public class SubscriptionManager : ISubscriptionManager
{
    public async static Task ManageSubscriptionCreated()
    {

    }

    private static Subscription CreateNewSubscription(EasyCartSubscriptionModel req, User userFromDb)
    {
        return new Subscription()
        {
            CustomerEmail = req.CustomerEmail,
            OrderId = req.OrderId,
            AmountPaid = req.AmountPaid,
            CreatedDate = DateTime.Now,
            UserId = userFromDb.Id,
            SubscriptionCurrentPeriodEnd = req.SubscriptionCurrentPeriodEnd,
            SubscriptionCurrentPeriodStart = req.SubscriptionCurrentPeriodStart,
        };
    }
}
