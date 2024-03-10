﻿using System.Collections.Concurrent;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.Services.PostsGenerator;
public class PostsGenerator(ITextGenerator postDescriptionGenerator, IImageGeneratingOrchestrator imageGenerating, string baseApiUrl) : IPostsGenerator
{
    private readonly int numberOfInitialPosts = 2;

    private readonly ConcurrentBag<Post> posts = [];

    public async Task<List<Post>> GenerateInitialPostsAsync(GeneratePostRequest request)
    {
        var tasks = Enumerable.Range(0, numberOfInitialPosts)
            .Select(async index =>
            {
                var description = await postDescriptionGenerator.GeneratePostDescription(request);
                var imageName = await imageGenerating.GenerateImage(request);

                var post = new Post()
                {
                    CreatedDate = DateTime.Now,
                    UserId = request.UserId,
                    Description = description,
                    Title = "",
                    ImageUrl = $"{baseApiUrl}/{imageName}",
                    IsPublished = false,
                    ShedulePublishDate = DateTime.Today.AddDays(index + 1),
                };

                posts.Add(post);
            });

        await Task.WhenAll(tasks);

#pragma warning disable IDE0305 // Simplify collection initialization
        return posts.ToList();
#pragma warning restore IDE0305 // Simplify collection initialization
    }
}
