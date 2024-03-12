using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.SocialMediaConnections;
using Sameposty.Services.PostsPublishers.FacebookPostsPublisher;
using Sameposty.Services.PostsPublishers.FacebookPostsPublisher.Models;

namespace Sameposty.API.Endpoints.Posts.PostNow;

public class PostNowEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IFacebookPostsPublisher facebookPostsPublisher) : Endpoint<PostNowRequest>
{
    public override void Configure()
    {
        Post("postNow");
    }

    public override async Task HandleAsync(PostNowRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var getPostQuery = new GetPostByIdQuery() { PostId = req.PostId };
        var post = await queryExecutor.ExecuteQuery(getPostQuery);

        var facebookConnectionQuery = new GetSocialMediaConnectionByUserId() { UserId = userId, Platform = SocialMediaConnection.SocialMediaPlatform.Facebook };
        var facebookConnection = await queryExecutor.ExecuteQuery(facebookConnectionQuery);

        var publishedPostId = await PostToFacebook(facebookPostsPublisher, post, facebookConnection);

        if (publishedPostId != null)
        {
            post.IsPublished = true;
            post.PublishedDate = DateTime.Now;
            post.PlatformPostId = publishedPostId;

            var updatePostCommand = new UpdatePostCommand() { Parameter = post };
            await commandExecutor.ExecuteCommand(updatePostCommand);

            await SendOkAsync(publishedPostId, ct);
        }
        else
        {
            ThrowError("Wystąpił błąd podczas publikowania posta. Spróbuj ponownie później.");
        }

    }

    private static async Task<string> PostToFacebook(IFacebookPostsPublisher facebookPostsPublisher, Post post, SocialMediaConnection facebookConnection)
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
