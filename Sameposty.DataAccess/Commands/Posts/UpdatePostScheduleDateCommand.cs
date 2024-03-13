using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Posts;
public class UpdatePostScheduleDateCommand(int postId, DateTime postScheduleDate) : CommandBase<Post, Post>
{
    public override async Task<Post> Execute(SamepostyDbContext db)
    {
        var postToUpdate = await db.Posts.FindAsync(postId);

        if (postToUpdate == null)
        {
            throw new ArgumentException(nameof(postToUpdate));
        }

        postToUpdate.ShedulePublishDate = postScheduleDate;

        db.Posts.Update(postToUpdate);
        await db.SaveChangesAsync();
        return postToUpdate;
    }
}
