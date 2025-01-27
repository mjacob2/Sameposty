using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;

namespace Sameposty.DataAccess.Queries.Posts;
public class GetUserPostsCountQuery(int userId) : QueryBase<int>
{
    public override async Task<int> Execute(SamepostyDbContext db)
    {
        return await db.Posts.Where(p => p.UserId == userId).CountAsync();
    }
}
