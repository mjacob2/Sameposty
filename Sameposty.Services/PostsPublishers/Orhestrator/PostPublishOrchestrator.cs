using Hangfire;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.FileRemover;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;
using Sameposty.Services.PostsPublishers.PostsPublisher;

namespace Sameposty.Services.PostsPublishers.Orhestrator;
public class PostPublishOrchestrator(IPostsPublisher postsPublisher, IImageSaver imageSaver, IFileRemover fileRemover, ICommandExecutor commandExecutor) : IPostPublishOrchestrator
{

    [AutomaticRetry(Attempts = 0)]
    public async Task<List<PublishResult>> PublishPostToAll(PublishPostToAllRequest request)
    {
        await MarkPostIsPublishingInProgress(request.Post);

        var publishingResults = await postsPublisher.PublishPost(request.Post);

        var imageThumbnailName = string.Empty;

        if (!string.IsNullOrEmpty(request.Post.ImageUrl))
        {
            imageThumbnailName = await imageSaver.DownsizePNG(request.Post.ImageUrl);

            fileRemover.RemovePostImage(request.Post.ImageUrl);
        }

        request.Post.PublishResults = publishingResults;

        BackgroundJob.Delete(request.Post.JobPublishId);

        await UpdatePost(request.Post, imageThumbnailName, request.BaseApiUrl);

        return publishingResults;
    }

    private async Task MarkPostIsPublishingInProgress(Post post)
    {
        post.IsPublishingInProgress = true;
        var updatePostCommand = new UpdatePostCommand() { Parameter = post };
        await commandExecutor.ExecuteCommand(updatePostCommand);
    }

    private async Task UpdatePost(Post post, string imageThumbnailUrl, string baseApiUrl)
    {
        post.Description = new string(post.Description.Take(150).ToArray()) + "...";
        post.IsPublished = true;
        post.JobPublishId = string.Empty;
        post.PublishedDate = GetNowInPoland();
        post.IsPublishingInProgress = false;
        post.ImageUrl = $"{baseApiUrl}/Thumbnails/{imageThumbnailUrl}";

        var updatePostCommand = new UpdatePostCommand() { Parameter = post };
        await commandExecutor.ExecuteCommand(updatePostCommand);
    }

    private static DateTime GetNowInPoland()
    {
        DateTime utcNow = DateTime.UtcNow;
        TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        DateTime cetTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, cetZone);
        return cetTime;
    }
}
