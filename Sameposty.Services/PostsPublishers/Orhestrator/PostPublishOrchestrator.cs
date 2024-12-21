using Hangfire;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.Services.FileRemoverService;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;
using Sameposty.Services.PostsPublishers.PostsPublisher;

namespace Sameposty.Services.PostsPublishers.Orhestrator;
public class PostPublishOrchestrator(IPostsPublisher postsPublisher, IImageSaver imageSaver, IFileRemover fileRemover, ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : IPostPublishOrchestrator
{

    [AutomaticRetry(Attempts = 0)]
    public async Task<List<PublishResult>> PublishPostToAll(PublishPostToAllRequest request)
    {
        var postToPublish = await queryExecutor.ExecuteQuery(new GetPostByIdQuery() { PostId = request.PostId }) ?? throw new Exception("We can not publish post that does not exist!");

        await MarkPostIsPublishingInProgress(postToPublish);

        var publishingResults = await postsPublisher.PublishPost(postToPublish);

        var imageThumbnailName = string.Empty;

        if (!string.IsNullOrEmpty(postToPublish.ImageUrl))
        {
            imageThumbnailName = await imageSaver.DownsizePNG(postToPublish.ImageUrl);

            fileRemover.RemovePostImage(postToPublish.ImageUrl);
        }

        postToPublish.PublishResults = publishingResults;

        BackgroundJob.Delete(postToPublish.JobPublishId);

        await UpdatePost(postToPublish, imageThumbnailName, request.BaseApiUrl);

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
