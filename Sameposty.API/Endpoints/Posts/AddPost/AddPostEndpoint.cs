using FastEndpoints;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.PostGeneratingManager;

namespace Sameposty.API.Endpoints.Posts.AddPost;

public class AddPostEndpoint(IQueryExecutor queryExecutor, IPostGeneratingManager manager) : Endpoint<AddPostRequest, Post>
{
    public override void Configure()
    {
        Post("posts");
    }
    public override async Task HandleAsync(AddPostRequest req, CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(loggedUserId));

        if (userFromDb.BasicInformation == null && userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            ThrowError("Nie podano informacji o firmie");
        }

        if (userFromDb.GetImageTokensLeft() < 1)
        {
            ThrowError("Zużyto wszystkie tokeny do generowania obrazów! Tokeny odnawiają się wraz z kolejnym okresem subskrypcji premium.");
        }

        if (userFromDb.GetTextTokensLeft() < 1)
        {
            ThrowError("Zużyto wszystkie tokeny do generowania tekstów! Tokeny odnawiają się wraz z kolejnym okresem subskrypcji premium.");
        }

        var post = await manager.ManageGeneratingSinglePost(userFromDb, req.Date);

        await SendOkAsync(post, ct);
    }
}
