using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsGenerator;
public class PostsGenerator : IPostsGenerator
{
    private int numberOfInitialPosts = 4;
    private readonly List<Post> posts = [];

    public List<Post> GenerateInitialPosts(int userId)
    {
        while (numberOfInitialPosts > 0)
        {
            var post = new Post()
            {
                CreatedDate = DateTime.Now,
                UserId = userId,
                Description = $"Some description {numberOfInitialPosts}",
                Title = "Jakiś taki fajny tytuł",
            };

            posts.Add(post);
            numberOfInitialPosts--;
        }

        return posts;
    }
}
