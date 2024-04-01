using FastEndpoints;
using Hangfire;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsPublishers.Orhestrator;

namespace Sameposty.API.Endpoints.Posts.AddInitalPosts;

public class AddInitialPostsEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IPostsGenerator postsGenerator, IHostEnvironment environment, IPostPublishOrchestrator postPublishOrchestrator, IConfigurator configurator) : EndpointWithoutRequest
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

        var posts = userFromDb.Email != "admin"
            ? await postsGenerator.GenerateInitialPostsAsync(generatePostRequest)
            : postsGenerator.GenerateStubbedPosts(generatePostRequest);

        foreach (var post in posts)
        {
            var request = new PublishPostToAllRequest()
            {
                BaseApiUrl = configurator.ApiBaseUrl,
                Post = post,
                Connections = new()
                {
                    FacebookConnection = userFromDb.FacebookConnection,
                    InstagramConnection = userFromDb.InstagramConnection,
                },

            };
            var jobPublishId = BackgroundJob.Schedule(() => postPublishOrchestrator.PublishPostToAll(request), new DateTimeOffset(post.ShedulePublishDate));

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
