using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Subscriptions;
public class AddSubscriptionCommand : CommandBase<Subscription, Subscription>
{
    public override async Task<Subscription> Execute(SamepostyDbContext db)
    {
        await db.Subscriptions.AddAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
