using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;
using static Sameposty.DataAccess.Entities.SocialMediaConnection;

namespace Sameposty.DataAccess.Queries.SocialMediaConnections;

public class GetSocialMediaConnectionByUserId : QueryBase<SocialMediaConnection>
{
    public required int UserId { get; set; }

    public required SocialMediaPlatform Platform { get; set; }

    public override async Task<SocialMediaConnection> Execute(SamepostyDbContext db)
    {
        return await db.SocialMediaConnections.Where(x => x.UserId == UserId && x.Platform == Platform).FirstOrDefaultAsync();
    }
}
