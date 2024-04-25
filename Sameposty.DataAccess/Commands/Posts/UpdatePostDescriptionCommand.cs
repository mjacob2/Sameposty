using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Posts;
public class UpdatePostDescriptionCommand(int postId, string postDescription) : CommandBase<Post, Post>
{
    public override async Task<Post> Execute(SamepostyDbContext db)
    {
        var postToUpdate = await db.Posts.FindAsync(postId);

        if (postToUpdate == null)
        {
            throw new ArgumentException(nameof(postToUpdate));
        }

        if (postToUpdate.IsPublishingInProgress || postToUpdate.IsPublished)
        {
            throw new Exception("Nie można już edytować tego posta");
        }

        postToUpdate.Description = postDescription;

        db.Posts.Update(postToUpdate);
        await db.SaveChangesAsync();
        return postToUpdate;
    }
}
