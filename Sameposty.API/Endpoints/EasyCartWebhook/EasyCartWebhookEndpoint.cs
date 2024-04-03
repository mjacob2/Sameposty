using FastEndpoints;
using Hangfire;
using Sameposty.DataAccess.Commands.Subscriptions;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;

namespace Sameposty.API.Endpoints.EasyCartWebhook;

public class EasyCartWebhookEndpoint(IQueryExecutor queryExecutor, IEmailService email, ICommandExecutor commandExecutor, IConfigurator configurator, IPostsGenerator postsGenerator, IPostPublishOrchestrator postPublishOrchestrator) : Endpoint<EasyCartRequest>
{
    public override void Configure()
    {
        Post("easycart");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EasyCartRequest req, CancellationToken ct)
    {
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByEmailQuery() { Email = "jakubicki.m@gmail.com" });

        if (userFromDb == null)
        {
            await email.SendNotifyInvalidSubscriptionEmail(req.CustomerEmail, req.CustomerId, req.OrderId);

            return;
        }

        if (req.Event == "subscription_created")
        {
            await ActForSubcriptionCreated(req, userFromDb);
        }

        await SendOkAsync(ct);
    }

    private async Task ActForSubcriptionCreated(EasyCartRequest req, User userFromDb)
    {
        var subscription = CreateNewSubscription(req, userFromDb);
        await SaveNewSubscription(subscription);
        UpdateUserTokens(userFromDb);
        var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
        var newPostsGenerated = await postsGenerator.GeneratePremiumPostsAsync(generatePostRequest);

        foreach (var post in newPostsGenerated)
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

        userFromDb.Posts = newPostsGenerated;

        if (userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            userFromDb.ImageTokensUsed += configurator.NumberFirstPostsGenerated;
            userFromDb.TextTokensUsed += configurator.NumberFirstPostsGenerated;
        }

        var updateUserCommand = new UpdateUserCommand() { Parameter = userFromDb };
        await commandExecutor.ExecuteCommand(updateUserCommand);

        await email.SendNotifyUserNewPostsCreatedEmail(userFromDb.Email);
    }

    private async Task SaveNewSubscription(Subscription subscription)
    {
        await commandExecutor.ExecuteCommand(new AddSubscriptionCommand() { Parameter = subscription });
    }

    private static GeneratePostRequest CreatePostGeneratingRequest(User userFromDb)
    {
        return new GeneratePostRequest()
        {
            UserId = userFromDb.Id,
            BrandName = userFromDb.BasicInformation.BrandName,
            Audience = userFromDb.BasicInformation.Audience,
            Mission = userFromDb.BasicInformation.Mission,
            ProductsAndServices = userFromDb.BasicInformation.ProductsAndServices,
            Goals = userFromDb.BasicInformation.Goals,
            Assets = userFromDb.BasicInformation.Assets,
        };
    }

    private async void UpdateUserTokens(User userFromDb)
    {
        userFromDb.ImageTokensLimit = configurator.ImageTokensPremiumLimit;
        userFromDb.TextTokensLimit = configurator.TextTokensPremiumLimit;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
    }

    private static Subscription CreateNewSubscription(EasyCartRequest req, User userFromDb)
    {
        return new Subscription()
        {
            CustomerEmail = req.CustomerEmail,
            OrderId = req.OrderId,
            AmountPaid = req.AmountPaid,
            CreatedDate = DateTime.Now,
            OrderHasInvoice = req.OrderHasInvoice,
            UserId = userFromDb.Id,
            SubscriptionCurrentPeriodEnd = req.SubscriptionCurrentPeriodEnd,
            SubscriptionCurrentPeriodStart = req.SubscriptionCurrentPeriodStart,
        };
    }
}
