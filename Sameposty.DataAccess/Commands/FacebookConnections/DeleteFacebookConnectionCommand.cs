using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.FacebookConnections;
public class DeleteFacebookConnectionCommand : CommandBase<FacebookConnection, FacebookConnection>
{
    public override async Task<FacebookConnection> Execute(SamepostyDbContext db)
    {
        db.FacebookConnections.Remove(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
