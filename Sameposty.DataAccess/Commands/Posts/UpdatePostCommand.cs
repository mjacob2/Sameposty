using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Posts;
public class UpdatePostCommand : CommandBase<Post, Post>
{
    public override async Task<Post> Execute(SamepostyDbContext db)
    {
        db.Posts.Update(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
