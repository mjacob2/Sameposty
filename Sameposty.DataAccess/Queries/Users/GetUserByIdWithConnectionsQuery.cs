using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Users;
public class GetUserByIdWithConnectionsQuery(int userId) : QueryBase<User>
{
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        return await db.Users
        .Include(u => u.FacebookConnection)
        .Include(u => u.InstagramConnection)
        .FirstOrDefaultAsync(u => u.Id == userId);
    }
}