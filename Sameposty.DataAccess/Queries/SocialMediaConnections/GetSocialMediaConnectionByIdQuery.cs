using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.SocialMediaConnections;
public class GetSocialMediaConnectionByIdQuery : QueryBase<SocialMediaConnection>
{
    public required int SocialMediaConnectionId { get; set; }

    public override async Task<SocialMediaConnection> Execute(SamepostyDbContext db)
    {
        return await db.SocialMediaConnections.FindAsync(SocialMediaConnectionId);
    }
}
