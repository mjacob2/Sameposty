using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.InstagramConenctions;
public class AddInstagramConnectionCommand : CommandBase<InstagramConnection, InstagramConnection>
{
    public override async Task<InstagramConnection> Execute(SamepostyDbContext db)
    {
        await db.InstagramConnections.AddAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
