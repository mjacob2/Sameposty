using System.Collections.Concurrent;
using Hangfire;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.Configurator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;

namespace Sameposty.Services.PostsGenerator;
public class PostsGenerator(ITextGenerator postDescriptionGenerator, IImageGeneratingOrchestrator imageGenerating, IConfigurator configurator, IPostPublishOrchestrator postPublishOrchestrator) : IPostsGenerator
{
    private readonly ConcurrentBag<Post> posts = [];

    public async Task<List<Post>> GeneratePostsAsync(GeneratePostRequest request, int numberOfPostsToGenerate)
    {
        var tasks = Enumerable.Range(0, numberOfPostsToGenerate)
            .Select(async index =>
            {
                request.ShedulePublishDate = DateTime.Today.AddDays(index + 2).Date.AddHours(9);

                var post = await GenerateSinglePost(request);
                posts.Add(post);
            });

        await Task.WhenAll(tasks);

        return posts.ToList();
    }

    private async Task<Post> GenerateSinglePost(GeneratePostRequest request)
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

        var publishRequest = new PublishPostToAllRequest()
        {
            BaseApiUrl = configurator.ApiBaseUrl,
            Post = post,

        };
        var jobPublishId = BackgroundJob.Schedule(() => postPublishOrchestrator.PublishPostToAll(publishRequest), new DateTimeOffset(post.ShedulePublishDate));

        post.JobPublishId = jobPublishId;

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
