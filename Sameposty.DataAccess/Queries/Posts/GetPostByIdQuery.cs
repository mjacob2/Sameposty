using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Posts;
public class GetPostByIdQuery : QueryBase<Post>
{
    public required int PostId { get; set; }
    public override async Task<Post> Execute(SamepostyDbContext db)
    {
        return await db.Posts.FindAsync(PostId);
    }
}
