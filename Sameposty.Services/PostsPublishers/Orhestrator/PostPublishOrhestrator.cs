using Hangfire;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.FileRemover;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsPublishers.PostsPublisher;

namespace Sameposty.Services.PostsPublishers.Orhestrator;
public class PostPublishOrhestrator(IPostsPublisher postsPublisher, IImageSaver imageSaver, IFileRemover fileRemover, ICommandExecutor commandExecutor) : IPostPublishOrhestrator
{

    [AutomaticRetry(Attempts = 0)]
    public async Task<List<PublishResult>> PublishPostToAll(Post post, List<SocialMediaConnection> connections, string baseApiUrl)
    {
        var publishingResults = await postsPublisher.PublishPost(post, connections);

        var imageThumbnailName = string.Empty;

        if (!string.IsNullOrEmpty(post.ImageUrl))
        {
            imageThumbnailName = await imageSaver.DownsizePNG(post.ImageUrl);

            fileRemover.RemovePostImage(post.ImageUrl);
        }

        post.PublishResults = publishingResults;

        await UpdatePost(post, imageThumbnailName, baseApiUrl);

        BackgroundJob.Delete(post.JobPublishId);

        return publishingResults;
    }

    private async Task UpdatePost(Post post, string imageThumbnailUrl, string baseApiUrl)
    {
        post.Description = new string(post.Description.Take(150).ToArray()) + "...";
        post.IsPublished = true;
        post.JobPublishId = string.Empty;
        post.PublishedDate = GetNowInPoland();
        post.PlatformPostId = "published";
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
