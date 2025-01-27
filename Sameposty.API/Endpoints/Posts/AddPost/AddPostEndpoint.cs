using FastEndpoints;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.ConfiguratorService;
using Sameposty.Services.PostGeneratingManager;

namespace Sameposty.API.Endpoints.Posts.AddPost;

public class AddPostEndpoint(IQueryExecutor queryExecutor, IPostGeneratingManager manager, IConfigurator config) : Endpoint<AddPostRequest, Post>
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

        if (userFromDb.Posts.Count >= userFromDb.PostsToGenerateLeft)
        {
            ThrowError($"Osiągnięto limit jednocześnie zaplanowanych postów - {userFromDb.PostsToGenerateLeft}. Opublikuj jakiś post lub usuń, by stworzyć nowy.");
        }

        if (userFromDb.BasicInformation.IsEmpty() && (req.GenerateText || req.GenerateImage))
        {
            ThrowError("Aby skorzystać z automatycznego generowania, uzupełnij informacje o stronie");
        }

        if (req.GenerateImage && userFromDb.ImageTokensLeft < 1)
        {
            ThrowError("Zużyto wszystkie tokeny do generowania obrazów.");
        }

        if (req.GenerateText && userFromDb.TextTokensLeft < 1)
        {
            ThrowError("Zużyto wszystkie tokeny do generowania tekstów.");
        }

        var post = await manager.GenerateSinglePost(userFromDb, req.Date, req.GenerateText, req.GenerateImage);

        await SendOkAsync(post, ct);
    }
}
