using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.PostsGenerator;

namespace Sameposty.API.Endpoints.Posts.AddInitalPosts;

public class AddInitialPostsEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IPostsGenerator postsGenerator, IConfigurator configurator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("posts/initial");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);
        var getUserFromDbQuery = new GetUserByIdQuery(id);
        var userFromDb = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        if (userFromDb.Privilege.CanGenerateInitialPosts == false && userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            ThrowError("Aby wygenerować więcej wspaniałych postów, zapraszamy do skorzystania z abonamentu.");
        }

        if (userFromDb.BasicInformation == null && userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            ThrowError("Nie podano informacji o firmie");
        }

        if (userFromDb.GetImageTokensLeft() < configurator.NumberFirstPostsGenerated)
        {
            ThrowError("Brak wystarczającej ilości tokenów do generowania obrazów!");
        }

        if (userFromDb.GetTextTokensLeft() < configurator.NumberFirstPostsGenerated)
        {
            ThrowError("Brak wystarczającej ilości tokenów do generowania tekstów!");
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
        };

        var newPostsGenerated = userFromDb.Email != "admin"
            ? await postsGenerator.GeneratePostsAsync(generatePostRequest, configurator.NumberFirstPostsGenerated)
            : postsGenerator.GenerateStubbedPosts(generatePostRequest);

        userFromDb.Posts = newPostsGenerated;

        if (userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            userFromDb.ImageTokensUsed += configurator.NumberFirstPostsGenerated;
            userFromDb.TextTokensUsed += configurator.NumberFirstPostsGenerated;
        }

        if (userFromDb.Role == DataAccess.Entities.Roles.FreeUser)
        {
            userFromDb.Privilege.CanGenerateInitialPosts = false;
        }

        var updateUserCommand = new UpdateUserCommand() { Parameter = userFromDb };
        await commandExecutor.ExecuteCommand(updateUserCommand);

        await SendOkAsync(newPostsGenerated, ct);
    }
}
