using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.PostsGenerator;

namespace Sameposty.API.Endpoints.Posts.AddInitalPosts;

public class AddInitialPostsEndpoint(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IPostsGenerator postsGenerator, IHostEnvironment environment) : EndpointWithoutRequest
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

        if (environment.IsProduction())
        {
            posts = await postsGenerator.GenerateInitialPostsAsync(generatePostRequest);
        }
        else
        {
            posts = AddStubbedPosts(id);
        }

        userFromDb.Posts = posts;

        var updateUserCommand = new UpdateUserCommand() { Parameter = userFromDb };
        await commandExecutor.ExecuteCommand(updateUserCommand);

        await SendOkAsync(posts, ct);
    }

    private static List<Post> AddStubbedPosts(int userId)
    {
        var posts = new List<Post>();

        var post = new Post()
        {
            CreatedDate = DateTime.Now,
            UserId = userId,
            Description = "",
            Title = "",
            ImageUrl = $"",
            IsPublished = false,
            ShedulePublishDate = DateTime.Today,
        };

        posts.Add(post);

        return posts;
    }
}
