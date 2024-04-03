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
                request.ShedulePublishDate = DateTime.Today.AddDays(index + 2).Date.AddHours(9);

                var post = await GeneratePost(request);
                posts.Add(post);
            });

        await Task.WhenAll(tasks);

        return posts.ToList();
    }

    public async Task<List<Post>> GeneratePremiumPostsAsync(GeneratePostRequest request)
    {
        var tasks = Enumerable.Range(0, configurator.NumberPremiumPostsGenerated)
            .Select(async index =>
            {
                request.ShedulePublishDate = DateTime.Today.AddDays(index + 2).Date.AddHours(9);

                var post = await GeneratePost(request);
                posts.Add(post);
            });

        await Task.WhenAll(tasks);

        return posts.ToList();
    }

    public async Task<Post> GeneratePost(GeneratePostRequest request)
    {
        var descriptionTask = postDescriptionGenerator.GeneratePostDescription(request);
        var imageTask = imageGenerating.GenerateImage(request.ProductsAndServices, 1);

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
            ShedulePublishDate = request.ShedulePublishDate,
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
