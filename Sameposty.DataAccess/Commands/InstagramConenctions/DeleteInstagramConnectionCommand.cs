using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.InstagramConenctions;
public class DeleteInstagramConnectionCommand : CommandBase<InstagramConnection, InstagramConnection>
{
    public override async Task<InstagramConnection> Execute(SamepostyDbContext db)
    {
        db.InstagramConnections.Remove(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
