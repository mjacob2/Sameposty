using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Users;
public class GetUserWithBasicInformationsByIdQuery(int userId) : QueryBase<User>
{
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        return await db.Users
       .Include(u => u.BasicInformation)
       .FirstOrDefaultAsync(u => u.Id == userId);
    }
}