using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.SocialMediaConnections;
public class GetSocialMediaConnectionsByUserIdQuery(int userId) : QueryBase<List<SocialMediaConnection>>
{
    public override async Task<List<SocialMediaConnection>> Execute(SamepostyDbContext db)
    {
        return await db.SocialMediaConnections.Where(x => x.UserId == userId).ToListAsync();
    }
}
