using Hangfire;
using Sameposty.DataAccess.Commands.Subscriptions;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsPublishers.Orhestrator;
using Sameposty.Services.PostsPublishers.Orhestrator.Models;

namespace Sameposty.Services.EasyCart;
public class EasyCart(IPostPublishOrchestrator postPublishOrchestrator, IPostsGenerator postsGenerator, IConfigurator configurator, ICommandExecutor commandExecutor, IEmailService email) : IEasyCart
{
    public async Task ActForSubcriptionCreated(EasyCartSubscriptionModel req, User userFromDb)
    {
        var subscription = CreateNewSubscription(req, userFromDb);
        await SaveNewSubscription(subscription);
        UpdateUserTokens(userFromDb);
        var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
        var newPostsGenerated = await postsGenerator.GeneratePremiumPostsAsync(generatePostRequest);
        SchedulePublish(userFromDb, newPostsGenerated);
        await UpdateUser(userFromDb, newPostsGenerated);
        await email.SendNotifyUserNewPostsCreatedEmail(userFromDb.Email);
    }

    public async Task ActForSubcriptionReneved(EasyCartSubscriptionModel req, User userFromDb)
    {

        await UpdateSubscription(req, userFromDb);
        UpdateUserTokens(userFromDb);
        var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
        var newPostsGenerated = await postsGenerator.GeneratePremiumPostsAsync(generatePostRequest);
        SchedulePublish(userFromDb, newPostsGenerated);
        await UpdateUser(userFromDb, newPostsGenerated);
        await email.SendNotifyUserNewPostsCreatedEmail(userFromDb.Email);
    }

    public async Task ActForSubcriptionCanceled(EasyCartSubscriptionModel req, User userFromDb)
    {

    }

    private async Task UpdateSubscription(EasyCartSubscriptionModel req, User user)
    {
        user.Subscription.SubscriptionCurrentPeriodStart = req.SubscriptionCurrentPeriodStart;
        user.Subscription.SubscriptionCurrentPeriodEnd = req.SubscriptionCurrentPeriodEnd;
        user.Subscription.AmountPaid = req.AmountPaid;
        user.Subscription.OrderHasInvoice = req.OrderHasInvoice;
        user.Subscription.OrderId = req.OrderId;
        user.Subscription.CustomerEmail = req.CustomerEmail;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });
    }

    private async Task UpdateUser(User userFromDb, List<Post> newPostsGenerated)
    {
        userFromDb.Posts = newPostsGenerated;

        if (userFromDb.Role != Roles.Admin)
        {
            userFromDb.ImageTokensUsed += configurator.NumberFirstPostsGenerated;
            userFromDb.TextTokensUsed += configurator.NumberFirstPostsGenerated;
            userFromDb.Role = Roles.PaidUser;
        }

        var updateUserCommand = new UpdateUserCommand() { Parameter = userFromDb };
        await commandExecutor.ExecuteCommand(updateUserCommand);
    }

    private void SchedulePublish(User userFromDb, List<Post> newPostsGenerated)
    {
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

    private static Subscription CreateNewSubscription(EasyCartSubscriptionModel req, User userFromDb)
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
