using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Posts;
public class AddListOfPostsCommand : CommandBase<List<Post>, List<Post>>
{
    public override async Task<List<Post>> Execute(SamepostyDbContext db)
    {
        await db.Posts.AddRangeAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
