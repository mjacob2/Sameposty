using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.SocialMediaConnections;

public class GetSocialMediaConnectionsByUserId : QueryBase<List<SocialMediaConnection>>
{
    public required int UserId { get; set; }

    public required SocialMediaPlatform Platform { get; set; }

    public override async Task<List<SocialMediaConnection>> Execute(SamepostyDbContext db)
    {
        return await db.SocialMediaConnections.Where(x => x.UserId == UserId).ToListAsync();
    }
}
