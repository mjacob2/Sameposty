using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.FacebookConnections;
public class GetFacebookConnectionByIdQuery(int id) : QueryBase<FacebookConnection>
{
    public override async Task<FacebookConnection> Execute(SamepostyDbContext db)
    {
        return await db.FacebookConnections.FirstOrDefaultAsync(x => x.UserId == id);
    }
}
