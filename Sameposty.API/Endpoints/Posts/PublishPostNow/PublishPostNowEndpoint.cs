using FastEndpoints;
using Sameposty.API.Endpoints.Posts.PostNow;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.SocialMediaConnections;
using Sameposty.Services.FileRemover;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsPublishers.FacebookPostsPublisher;
using Sameposty.Services.PostsPublishers.FacebookPostsPublisher.Models;

namespace Sameposty.API.Endpoints.Posts.PublishPostNow;

public class PublishPostNowEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IFacebookPostsPublisher facebookPostsPublisher, IFileRemover fileRemover, IImageSaver imageSaver) : Endpoint<PublishPostNowRequest>
{
    public override void Configure()
    {
        Post("postNow");
    }

    public override async Task HandleAsync(PublishPostNowRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var getPostQuery = new GetPostByIdQuery() { PostId = req.PostId };
        var post = await queryExecutor.ExecuteQuery(getPostQuery);

        if (string.IsNullOrEmpty(post.ImageUrl))
        {
            ThrowError("Post nie ma zdjęcia!");
        }

        if (string.IsNullOrEmpty(post.Description))
        {
            ThrowError("Post nie ma opisu!");
        }

        var facebookConnectionQuery = new GetSocialMediaConnectionByUserId() { UserId = userId, Platform = SocialMediaConnection.SocialMediaPlatform.Facebook };
        var facebookConnection = await queryExecutor.ExecuteQuery(facebookConnectionQuery);

        if (facebookConnection == null)
        {
            ThrowError("Najpierw połącz się z platformą social media!");
        }

        var publishedPostId = await PublishToFacebook(facebookPostsPublisher, post, facebookConnection);

        if (publishedPostId != null)
        {
            var imageThumbnailUrl = await imageSaver.DownsizePNG(post.ImageUrl);

            fileRemover.RemovePostImage(post.ImageUrl);

            post.IsPublished = true;
            post.PublishedDate = DateTime.Now;
            post.PlatformPostId = publishedPostId;
            post.ImageUrl = imageThumbnailUrl;

            var updatePostCommand = new UpdatePostCommand() { Parameter = post };
            await commandExecutor.ExecuteCommand(updatePostCommand);

            await SendOkAsync(publishedPostId, ct);
        }
        else
        {
            ThrowError("Wystąpił błąd podczas publikowania posta. Spróbuj ponownie później.");
        }

    }

    private static async Task<string> PublishToFacebook(IFacebookPostsPublisher facebookPostsPublisher, Post post, SocialMediaConnection facebookConnection)
    {
        var postToPublish = new FacegookPostToPublish()
        {
            ImageUrl = post.ImageUrl,
            Message = post.Description,
        };

        var pageInfo = new FacebookPageInfo()
        {
            LongLivedPageAccessToken = facebookConnection.AccesToken,
            PageId = facebookConnection.PageId,
        };

        return await facebookPostsPublisher.PublishPost(postToPublish, pageInfo);
    }
}
