using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Posts;
public class GetArchivedPostsByUserId : QueryBase<List<Post>>
{
    public required int UserId { get; set; }

    public override async Task<List<Post>> Execute(SamepostyDbContext db)
    {
        return await db.Posts.Where(x => x.UserId == UserId && x.IsPublished)
        .OrderByDescending(x => x.PublishedDate)
        .Include(x => x.PublishResults)
        .ToListAsync();
    }
}
