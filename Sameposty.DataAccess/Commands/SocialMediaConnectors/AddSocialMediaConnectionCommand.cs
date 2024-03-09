using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.SocialMediaConnectors;
public class AddSocialMediaConnectionCommand : CommandBase<SocialMediaConnection, SocialMediaConnection>
{
    public override async Task<SocialMediaConnection> Execute(SamepostyDbContext db)
    {
        await db.SocialMediaConnections.AddAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
