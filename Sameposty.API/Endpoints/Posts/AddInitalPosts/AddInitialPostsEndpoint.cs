using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.ConfiguratorService;
using Sameposty.Services.PostGeneratingManager;

namespace Sameposty.API.Endpoints.Posts.AddInitalPosts;

public class AddInitialPostsEndpoint(IQueryExecutor queryExecutor, IPostGeneratingManager manager, IConfigurator configurator, ICommandExecutor commandExecutor) : EndpointWithoutRequest
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

        if (userFromDb.BasicInformation == null && userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            ThrowError("Nie podano informacji o firmie");
        }

        if (userFromDb.ImageTokensLeft < 1)
        {
            ThrowError("Brak wystarczającej ilości tokenów do generowania obrazów!");
        }

        if (userFromDb.TextTokensLeft < 1)
        {
            ThrowError("Brak wystarczającej ilości tokenów do generowania tekstów!");
        }

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });

        var posts = await manager.GenerateNumberOfPosts(userFromDb, 1);

        await SendOkAsync(posts, ct);
    }
}
