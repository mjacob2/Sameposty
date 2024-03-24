using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.InstagramConnections;
public class GetInstagramConnectionByUserIdQuery(int userId) : QueryBase<InstagramConnection>
{
    public override async Task<InstagramConnection> Execute(SamepostyDbContext db)
    {
        return await db.InstagramConnections.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
