using System.Collections.Concurrent;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.Configurator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.Services.PostsGenerator;
public class PostsGenerator(ITextGenerator postDescriptionGenerator, IImageGeneratingOrchestrator imageGenerating, IConfigurator configurator) : IPostsGenerator
{
    private readonly ConcurrentBag<Post> posts = [];

    public async Task<List<Post>> GeneratePostsAsync(GeneratePostRequest originalRequest, int numberOfPostsToGenerate)
    {
        var tasks = Enumerable.Range(1, numberOfPostsToGenerate)
            .Select(async index =>
            {
                var newRequest = new GeneratePostRequest()
                {
                    Assets = originalRequest.Assets,
                    Audience = originalRequest.Audience,
                    UserId = originalRequest.UserId,
                    BrandName = originalRequest.BrandName,
                    Goals = originalRequest.Goals,
                    Mission = originalRequest.Mission,
                    ProductsAndServices = originalRequest.ProductsAndServices,
                };
                TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(index * 2).Date.AddHours(7), cetZone);
                newRequest.ShedulePublishDate = localDateTime;

                var post = await GenerateSinglePost(newRequest);
                posts.Add(post);
            });

        await Task.WhenAll(tasks);

        return posts.ToList();
    }

    public async Task<Post> GenerateSinglePost(GeneratePostRequest request)
    {
        var descriptionTask = postDescriptionGenerator.GeneratePostDescription(request);
        var imageTask = imageGenerating.GenerateImage(request.ProductsAndServices, 1, request.GenerateImage);

        await Task.WhenAll(descriptionTask, imageTask);

        var description = descriptionTask.Result;
        var imageName = imageTask.Result;

        var post = new Post()
        {
            CreatedDate = DateTime.Now,
            UserId = request.UserId,
            Description = description,
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
            ImageUrl = $"",
            IsPublished = false,
            ShedulePublishDate = DateTime.Today.AddDays(1).Date.AddHours(9),
        };

        posts.Add(post1);

        return posts;
    }
}
