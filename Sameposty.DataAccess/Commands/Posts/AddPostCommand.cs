using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Posts;
public class AddPostCommand : CommandBase<Post, Post>
{
    public override async Task<Post> Execute(SamepostyDbContext db)
    {
        await db.Posts.AddAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
