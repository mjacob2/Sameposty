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
using Sameposty.Services.Stripe;
using Stripe;

namespace Sameposty.Services.SubscriptionManager;
public class SubscriptionManager(IStripeService stripeService, ICommandExecutor commandExecutor, IConfigurator configurator, IPostsGenerator postsGenerator, IPostPublishOrchestrator postPublishOrchestrator, IEmailService email) : ISubscriptionManager
{
    public async Task ManageSubscriptionCreated(User userFromDb, string cardTokenId)
    {
        var stripeCustomer = await CreateStripeCustomer(stripeService, userFromDb, cardTokenId);
        var subscription = await stripeService.CreateSubscription(stripeCustomer.Id);
        var sqlSubscription = CreateNewSubscription(userFromDb.Id, subscription.Id, subscription.CurrentPeriodStart, subscription.CurrentPeriodEnd, subscription.CustomerId, subscription.Items.Data[0].Plan.Amount);
        await SaveNewSubscription(sqlSubscription);
        await UpdateUserTokens(userFromDb);


        // te zadania przenieść do webhooka po ustorzeniu nowej subskrypcji
     //   var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
     //   var newPostsGenerated = await postsGenerator.GeneratePremiumPostsAsync(generatePostRequest);
     //   SchedulePublish(userFromDb, newPostsGenerated);
     //   await UpdateUser(userFromDb, newPostsGenerated);
     //   await email.SendNotifyUserNewPostsCreatedEmail(userFromDb.Email);

        // wysłać fakturę do kleinta!
    }

    private static async Task<Customer> CreateStripeCustomer(IStripeService stripeService, User userFromDb, string cardTokenId)
    {
        var createStripeCustomerRequest = new CreateStripeCustomerRequest()
        {
            CardTokenId = cardTokenId,
            City = userFromDb.City,
            Email = userFromDb.Email,
            Name = userFromDb.Name,
            NIP = userFromDb.NIP,
            PostalCode = userFromDb.PostCode,
            Street = userFromDb.Street,
        };
        var stripeCustomer = await stripeService.CreateStripeCustomerCustomer(createStripeCustomerRequest);

        return stripeCustomer;
    }

    private static DataAccess.Entities.Subscription CreateNewSubscription(int userId, string subscriptionId, DateTime currentPeriodStart, DateTime currentPeriodEnd, string customerId, long? amountPaid)
    {
        return new DataAccess.Entities.Subscription()
        {
            AmountPaid = amountPaid / 100 ?? 0,
            CreatedDate = DateTime.Now,
            UserId = userId,
            SubscriptionCurrentPeriodEnd = currentPeriodEnd.ToString(),
            SubscriptionCurrentPeriodStart = currentPeriodStart.ToString(),
            StripeCusomerId = customerId,
            StipeSubscriptionId = subscriptionId,
        };
    }

    private async Task SaveNewSubscription(DataAccess.Entities.Subscription subscription)
    {
        await commandExecutor.ExecuteCommand(new AddSubscriptionCommand() { Parameter = subscription });
    }

    private async Task UpdateUserTokens(User userFromDb)
    {
        userFromDb.ImageTokensLimit = configurator.ImageTokensPremiumLimit;
        userFromDb.TextTokensLimit = configurator.TextTokensPremiumLimit;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
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

    private async Task UpdateUser(User userFromDb, List<Post> newPostsGenerated)
    {
        List<Post> currentPosts = userFromDb.Posts;
        currentPosts.AddRange(newPostsGenerated);
        userFromDb.Posts = currentPosts;

        if (userFromDb.Role != Roles.Admin)
        {
            userFromDb.ImageTokensUsed = configurator.NumberFirstPostsGenerated;
            userFromDb.TextTokensUsed = configurator.NumberFirstPostsGenerated;
            userFromDb.Role = Roles.PaidUser;
        }

        var updateUserCommand = new UpdateUserCommand() { Parameter = userFromDb };
        await commandExecutor.ExecuteCommand(updateUserCommand);
    }


}
