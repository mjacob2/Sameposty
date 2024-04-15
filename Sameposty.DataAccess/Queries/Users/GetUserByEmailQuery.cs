using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Users;
public class GetUserByEmailQuery(string email) : QueryBase<User>
{
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        return await db.Users
            .Include(u => u.BasicInformation)
            .Include(u => u.Subscription)
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}
