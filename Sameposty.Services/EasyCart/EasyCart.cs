using Sameposty.DataAccess.Commands.Subscriptions;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.PostsPublishers.Orhestrator;

namespace Sameposty.Services.EasyCart;
public class EasyCart(IPostPublishOrchestrator postPublishOrchestrator, IPostsGenerator postsGenerator, IConfigurator configurator, ICommandExecutor commandExecutor, IEmailService email) : IEasyCart
{
    public async Task ActForSubcriptionCreated(EasyCartSubscriptionModel req, User userFromDb)
    {
        var subscription = CreateNewSubscription(req, userFromDb);
        await SaveNewSubscription(subscription);
        UpdateUserTokens(userFromDb);
        var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
        var newPostsGenerated = await postsGenerator.GeneratePostsAsync(generatePostRequest, configurator.NumberPremiumPostsGenerated);
        await email.EmailUserNewPostsGenerated(userFromDb.Email);
    }

    public async Task ActForSubcriptionReneved(EasyCartSubscriptionModel req, User userFromDb)
    {

        await UpdateSubscription(req, userFromDb);
        UpdateUserTokens(userFromDb);
        var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
        var newPostsGenerated = await postsGenerator.GeneratePostsAsync(generatePostRequest, configurator.NumberPremiumPostsGenerated);
        await email.EmailUserNewPostsGenerated(userFromDb.Email);
    }

    public async Task ActForSubcriptionCanceled(EasyCartSubscriptionModel req, User userFromDb)
    {

    }

    private async Task UpdateSubscription(EasyCartSubscriptionModel req, User user)
    {
        user.Subscription.SubscriptionCurrentPeriodStart = req.SubscriptionCurrentPeriodStart;
        user.Subscription.SubscriptionCurrentPeriodEnd = req.SubscriptionCurrentPeriodEnd;
        user.Subscription.LastAmountPaid = req.AmountPaid;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });
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
            LastAmountPaid = req.AmountPaid,
            CreatedDate = DateTime.Now,
            UserId = userFromDb.Id,
            SubscriptionCurrentPeriodEnd = req.SubscriptionCurrentPeriodEnd,
            SubscriptionCurrentPeriodStart = req.SubscriptionCurrentPeriodStart,
        };
    }
}
