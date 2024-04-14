using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Subscriptions;
public class DeleteSubscriptionCommand : CommandBase<Subscription, Subscription>
{
    public override async Task<Subscription> Execute(SamepostyDbContext db)
    {
        db.Subscriptions.Remove(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
