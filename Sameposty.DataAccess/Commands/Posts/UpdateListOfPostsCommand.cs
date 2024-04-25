using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Posts;
public class UpdateListOfPostsCommand : CommandBase<List<Post>, List<Post>>
{
    public override async Task<List<Post>> Execute(SamepostyDbContext db)
    {
        db.Posts.UpdateRange(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
