using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.PostDescriptionGenerator;

namespace Sameposty.Services.PostsGenerator;
public class PostsGenerator(IPostDescriptionGenerator postDescriptionGenerator, IImageGeneratingOrchestrator imageGenerating) : IPostsGenerator
{
    private int numberOfInitialPosts = 1;

    private readonly List<Post> posts = [];

    public async Task<List<Post>> GenerateInitialPostsAsync(int userId, string companyDescription)
    {
        while (numberOfInitialPosts > 0)
        {
            var description = await postDescriptionGenerator.GeneratePostDescription(companyDescription);
            var imageName = await imageGenerating.GenerateImage(companyDescription);

            var post = new Post()
            {
                CreatedDate = DateTime.Now,
                UserId = userId,
                Description = description,
                Title = "Jakiś taki fajny tytuł",
                ImageUrl = $"https://localhost:7109/{imageName}",
            };

            posts.Add(post);
            numberOfInitialPosts--;
        }

        return posts;
    }
}
