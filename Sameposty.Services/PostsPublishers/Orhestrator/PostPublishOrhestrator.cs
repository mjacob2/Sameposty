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
    public async Task<List<PublishResult>> PublishPostToAll(Post post, List<SocialMediaConnection> connections)
    {
        var publishingResults = await postsPublisher.PublishPost(post, connections);

        var imageThumbnailUrl = await imageSaver.DownsizePNG(post.ImageUrl);

        fileRemover.RemovePostImage(post.ImageUrl);

        post.PublishResults = publishingResults;

        await UpdatePost(post, imageThumbnailUrl);

        return publishingResults;
    }

    private async Task UpdatePost(Post post, string imageThumbnailUrl)
    {
        post.IsPublished = true;
        post.PublishedDate = DateTime.Now;
        post.PlatformPostId = "published";
        post.ImageUrl = imageThumbnailUrl;

        var updatePostCommand = new UpdatePostCommand() { Parameter = post };
        await commandExecutor.ExecuteCommand(updatePostCommand);
    }
}
