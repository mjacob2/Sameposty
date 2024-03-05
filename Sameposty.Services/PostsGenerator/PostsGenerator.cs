using System.Collections.Concurrent;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.Services.PostsGenerator;
public class PostsGenerator(ITextGenerator postDescriptionGenerator, IImageGeneratingOrchestrator imageGenerating) : IPostsGenerator
{
    private readonly int numberOfInitialPosts = 4;

    private readonly ConcurrentBag<Post> posts = [];

    public async Task<List<Post>> GenerateInitialPostsAsync(int userId, string companyDescription)
    {
        var tasks = Enumerable.Range(0, numberOfInitialPosts)
            .Select(async _ =>
            {
                //var description = await postDescriptionGenerator.GeneratePostDescription(companyDescription);
                //var imageName = await imageGenerating.GenerateImage(companyDescription);

                var description = "";
                var imageName = "SampleImageName";

                var post = new Post()
                {
                    CreatedDate = DateTime.Now,
                    UserId = userId,
                    Description = description,
                    Title = "Jakiś taki fajny tytuł",
                    //ImageUrl = $"https://localhost:7109/{imageName}",
                    ImageUrl = "https://sameposty-api.azurewebsites.net/AddImage.png"
                };

                posts.Add(post);
            });

        await Task.WhenAll(tasks);

        return posts.ToList();
    }
}
