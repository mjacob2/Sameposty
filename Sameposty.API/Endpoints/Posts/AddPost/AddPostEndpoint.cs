using FastEndpoints;
using Hangfire;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;

namespace Sameposty.API.Endpoints.Posts.AddPost;

public class AddPostEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IPostsGenerator postsGenerator, IPostPublishOrchestrator postPublishOrchestrator, IConfigurator configurator) : Endpoint<AddPostRequest, Post>
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
            ThrowError("Brak wystarczającej ilości tokenów do generowania obrazów!");
        }

        if (userFromDb.GetTextTokensLeft() < 1)
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
            ShedulePublishDate = req.Date,
        };

        var newPostGenerated = await postsGenerator.GeneratePost(generatePostRequest);

        var request = new PublishPostToAllRequest()
        {
            BaseApiUrl = configurator.ApiBaseUrl,
            Post = newPostGenerated,
            Connections = new()
            {
                FacebookConnection = userFromDb.FacebookConnection,
                InstagramConnection = userFromDb.InstagramConnection,
            },
        };

        var jobPublishId = BackgroundJob.Schedule(() => postPublishOrchestrator.PublishPostToAll(request), new DateTimeOffset(newPostGenerated.ShedulePublishDate));

        newPostGenerated.JobPublishId = jobPublishId;

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
