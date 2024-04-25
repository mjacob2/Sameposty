using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostGeneratingManager;
public interface IPostGeneratingManager
{
    Task<Post> ManageGeneratingSinglePost(User user, DateTime date);
    Task<List<Post>> ManageGeneratingPosts(User user, int numberOfPostsToGenerate);
}
