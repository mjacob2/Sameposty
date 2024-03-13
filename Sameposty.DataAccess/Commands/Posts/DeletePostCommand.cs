using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Posts;
public class DeletePostCommand : CommandBase<Post, Post>
{
    public override async Task<Post> Execute(SamepostyDbContext db)
    {
        db.Posts.Remove(Parameter);
        await db.SaveChangesAsync();

        return Parameter;
    }
}
