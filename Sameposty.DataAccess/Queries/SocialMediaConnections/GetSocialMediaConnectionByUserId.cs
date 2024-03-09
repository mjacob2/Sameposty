using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.SocialMediaConnections;

public class GetSocialMediaConnectionByUserId : QueryBase<SocialMediaConnection>
{
    public required int UserId { get; set; }

    public override async Task<SocialMediaConnection> Execute(SamepostyDbContext db)
    {
        return await db.SocialMediaConnections.FirstOrDefaultAsync(x => x.UserId == UserId);
    }
}
