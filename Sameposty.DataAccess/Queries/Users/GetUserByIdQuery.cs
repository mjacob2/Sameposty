using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Users;
public class GetUserByIdQuery(int id) : QueryBase<User>
{
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        return await db.Users
    .Include(u => u.Posts)
    .ThenInclude(p => p.PublishResults)
    .Include(u => u.FacebookConnection)
    .Include(u => u.InstagramConnection)
    .Include(u => u.BasicInformation)
    .Include(u => u.Privilege)
    .Include(u => u.Subscription)
    .FirstOrDefaultAsync(u => u.Id == id);
    }
}
