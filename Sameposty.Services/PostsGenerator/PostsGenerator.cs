using System.Collections.Concurrent;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.Configurator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.Services.PostsGenerator;
public class PostsGenerator(ITextGenerator postDescriptionGenerator, IImageGeneratingOrchestrator imageGenerating, IConfigurator configurator) : IPostsGenerator
{
    private readonly ConcurrentBag<Post> posts = [];

    public async Task<List<Post>> GenerateInitialPostsAsync(GeneratePostRequest request)
    {
        var tasks = Enumerable.Range(0, configurator.NumberFirstPostsGenerated)
            .Select(async index =>
            {
                var post = await GeneratePost(request, index + 1);
                posts.Add(post);
            });

        await Task.WhenAll(tasks);

        return posts.ToList();
    }

    public async Task<Post> GeneratePost(GeneratePostRequest request, int index = 0)
    {
        var descriptionTask = postDescriptionGenerator.GeneratePostDescription(request);
        var imageTask = imageGenerating.GenerateImage(request);

        await Task.WhenAll(descriptionTask, imageTask);

        var description = await descriptionTask;
        var imageName = await imageTask;

        var post = new Post()
        {
            CreatedDate = DateTime.Now,
            UserId = request.UserId,
            Description = description,
            Title = "",
            ImageUrl = $"{configurator.ApiBaseUrl}/{imageName}",
            IsPublished = false,
            ShedulePublishDate = DateTime.Today.AddDays(index).Date.AddHours(9),
        };

        return post;
    }

    public List<Post> GenerateStubbedPosts(GeneratePostRequest request)
    {
        var posts = new List<Post>();

        var post1 = new Post()
        {
            CreatedDate = DateTime.Now,
            UserId = request.UserId,
            Description = "",
            Title = "",
            ImageUrl = $"",
            IsPublished = false,
            ShedulePublishDate = DateTime.Today.AddDays(1).Date.AddHours(9),
        };

        posts.Add(post1);

        return posts;
    }
}
