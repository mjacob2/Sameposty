using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Users;
public class GetAllUsersQuery : QueryBase<List<User>>
{
    public override async Task<List<User>> Execute(SamepostyDbContext db)
    {
        return await db.Users
            .Include(x => x.BasicInformation)
            .Include(x => x.FacebookConnection)
            .Include(x => x.InstagramConnection)
            .Include(x => x.Subscription)
            .Include(x => x.Privilege)
            .Include(x => x.Posts)
            .Include(x => x.Invoices)
            .ToListAsync();
    }
}
