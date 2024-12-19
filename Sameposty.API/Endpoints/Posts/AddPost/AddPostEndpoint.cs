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

        if (userFromDb.PostsToGenerateLeft < 1)
        {
            ThrowError("Zużyto wszystkie tokeny do generowania postów. Należy dokupić tokeny.");
        }

        if (userFromDb.BasicInformation == null && userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            ThrowError("Nie podano informacji o firmie");
        }

        if (userFromDb.BasicInformation.IsEmpty() && (req.GenerateText || req.GenerateImage))
        {
            ThrowError("Aby skorzystać z automatycznego generowania, uzupełnij informacje o firmie");
        }

        if (userFromDb.ImageTokensLeft < 1 && req.GenerateImage)
        {
            ThrowError("Zużyto wszystkie tokeny do generowania obrazów. Należy dokupić tokeny.");
        }

        if (userFromDb.TextTokensLeft < 1 && req.GenerateText)
        {
            ThrowError("Zużyto wszystkie tokeny do generowania tekstów. Należy dokupić tokeny.");
        }

        var post = await manager.GenerateSinglePost(userFromDb, req.Date, req.GenerateImage, req.GenerateText);

        await SendOkAsync(post, ct);
    }
}
