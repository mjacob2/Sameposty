using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Users;
public class GetUserByEmailQuery : QueryBase<User>
{
    public required string Email { get; set; }
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        return await db.Users
            .Include(u => u.BasicInformation)
            .FirstOrDefaultAsync(x => x.Email == Email);
    }
}
