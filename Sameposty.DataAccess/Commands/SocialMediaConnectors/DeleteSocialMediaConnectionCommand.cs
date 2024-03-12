using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.SocialMediaConnectors;
public class DeleteSocialMediaConnectionCommand : CommandBase<SocialMediaConnection, SocialMediaConnection>
{
    public override async Task<SocialMediaConnection> Execute(SamepostyDbContext db)
    {
        db.SocialMediaConnections.Remove(Parameter);
        await db.SaveChangesAsync();

        return Parameter;
    }
}
