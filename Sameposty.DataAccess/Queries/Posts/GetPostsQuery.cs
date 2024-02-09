using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Posts;
public class GetPostsQuery : QueryBase<List<Post>>
{
    public override async Task<List<Post>> Execute(SamepostyDbContext db)
    {
        return await db.Posts.ToListAsync();
    }
}
