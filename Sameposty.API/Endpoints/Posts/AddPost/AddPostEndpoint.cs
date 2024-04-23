using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.PostsGenerator;

namespace Sameposty.API.Endpoints.Posts.AddPost;

public class AddPostEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IPostsGenerator postsGenerator) : Endpoint<AddPostRequest, Post>
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

        var generatePostRequest = new GeneratePostRequest()
        {
            UserId = userFromDb.Id,
            BrandName = userFromDb.BasicInformation.BrandName,
            Audience = userFromDb.BasicInformation.Audience,
            Mission = userFromDb.BasicInformation.Mission,
            ProductsAndServices = userFromDb.BasicInformation.ProductsAndServices,
            Goals = userFromDb.BasicInformation.Goals,
            Assets = userFromDb.BasicInformation.Assets,
            ShedulePublishDate = req.Date,
        };

        var newPostGenerated = await postsGenerator.GenerateSinglePost(generatePostRequest);

        if (userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            userFromDb.ImageTokensUsed++;
            userFromDb.TextTokensUsed++;
        }

        var addPostCommand = new AddPostCommand() { Parameter = newPostGenerated };
        var newPostAdded = await commandExecutor.ExecuteCommand(addPostCommand);

        await SendOkAsync(newPostAdded, ct);
    }
}
