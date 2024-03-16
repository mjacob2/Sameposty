using FastEndpoints;
using Hangfire;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsPublishers.Orhestrator;

namespace Sameposty.API.Endpoints.Posts.AddInitalPosts;

public class AddInitialPostsEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IPostsGenerator postsGenerator, IHostEnvironment environment, IPostPublishOrhestrator postPublisher) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("posts/initial");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);
        var getUserFromDbQuery = new GetUserByIdQuery() { Id = id };
        var userFromDb = await queryExecutor.ExecuteQuery(getUserFromDbQuery);

        if (userFromDb.Privilege.CanGenerateInitialPosts == false)
        {
            ThrowError("Aby wygenerować więcej wspaniałych postów, zapraszamy do skorzystania z abonamentu.");
        }

        if (userFromDb.BasicInformation == null)
        {
            ThrowError("Nie podano informacji o firmie");
        }

        var generatePostRequest = new GeneratePostRequest()
        {
            UserId = userFromDb.Id,
            Branch = userFromDb.BasicInformation.Branch,
            Assets = userFromDb.BasicInformation.Assets,
            ProductsAndServices = userFromDb.BasicInformation.ProductsAndServices,
            Goals = userFromDb.BasicInformation.Goals,
        };

        var posts = new List<Post>();

        posts = environment.IsProduction() && userFromDb.Email != "admin"
            ? await postsGenerator.GenerateInitialPostsAsync(generatePostRequest)
            : postsGenerator.GenerateStubbedPosts(generatePostRequest);

        foreach (var post in posts)
        {
            var jobPublishId = BackgroundJob.Schedule(() => postPublisher.PublishPostToAll(post, userFromDb.SocialMediaConnections), new DateTimeOffset(post.ShedulePublishDate));

            post.JobPublishId = jobPublishId;
        }


        userFromDb.Posts = posts;

        if (userFromDb.Role == DataAccess.Entities.Roles.FreeUser)
        {
            userFromDb.Privilege.CanGenerateInitialPosts = false;
        }

        var updateUserCommand = new UpdateUserCommand() { Parameter = userFromDb };
        await commandExecutor.ExecuteCommand(updateUserCommand);

        await SendOkAsync(posts, ct);
    }
}
