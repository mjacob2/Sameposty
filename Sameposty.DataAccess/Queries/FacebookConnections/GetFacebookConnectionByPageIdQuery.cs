using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.FacebookConnections;
public class GetFacebookConnectionByPageIdQuery(string pageId) : QueryBase<FacebookConnection>
{
    public override async Task<FacebookConnection> Execute(SamepostyDbContext db)
    {
        return await db.FacebookConnections.FirstOrDefaultAsync(x => x.PageId == pageId);
    }
}
