using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsGenerator;
public interface IPostsGenerator
{
    List<Post> GenerateInitialPosts(int userId);
}
