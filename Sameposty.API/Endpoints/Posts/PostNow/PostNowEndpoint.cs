using FastEndpoints;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Posts;
using Sameposty.DataAccess.Queries.SocialMediaConnections;
using Sameposty.Services.PostsPublishers.FacebookPostsPublisher;
using Sameposty.Services.PostsPublishers.FacebookPostsPublisher.Models;

namespace Sameposty.API.Endpoints.Posts.PostNow;

public class PostNowEndpoint(IQueryExecutor queryExecutor, IFacebookPostsPublisher facebookPostsPublisher) : Endpoint<PostNowRequest>
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

        var facebookConnectionQuery = new GetSocialMediaConnectionByUserId() { UserId = userId };
        var facebookConnection = await queryExecutor.ExecuteQuery(facebookConnectionQuery);

        var publishedPostId = await PostToFacebook(facebookPostsPublisher, post, facebookConnection); //TODO save this id to database for later

        await SendOkAsync(publishedPostId, ct);
    }

    private static async Task<string> PostToFacebook(IFacebookPostsPublisher facebookPostsPublisher, Post post, SocialMediaConnection facebookConnection)
    {
        var postToPublish = new FacegookPostToPublish()
        {
            //ImageUrl = post.ImageUrl,
            ImageUrl = "https://middlers.pl/images/Marek-Jakubicki-doradca-kredytowy-Wroclaw-2.png",
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
