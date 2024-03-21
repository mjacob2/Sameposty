using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.FacebookConnections;
public class AddFacebookConnectionCommand : CommandBase<FacebookConnection, FacebookConnection>
{
    public override async Task<FacebookConnection> Execute(SamepostyDbContext db)
    {
        await db.FacebookConnections.AddAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
