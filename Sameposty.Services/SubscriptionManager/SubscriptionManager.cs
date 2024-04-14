using Sameposty.DataAccess.Commands.Subscriptions;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.Configurator;
using Sameposty.Services.Stripe;
using Stripe;

namespace Sameposty.Services.SubscriptionManager;
public class SubscriptionManager(IStripeService stripeService, ICommandExecutor commandExecutor, IConfigurator configurator) : ISubscriptionManager
{
    public async Task ManageSubscriptionCreated(User userFromDb, string cardTokenId)
    {
        if (userFromDb.Subscription != null)
        {
            throw new ArgumentException("Ten klient już ma subskrypcję!");
        }

        var stripeCustomer = await CreateStripeCustomer(stripeService, userFromDb, cardTokenId);
        var subscription = await stripeService.CreateSubscription(stripeCustomer.Id, userFromDb.Id.ToString());
        var sqlSubscription = CreateNewSubscription(userFromDb.Id, subscription.Id, subscription.CurrentPeriodStart, subscription.CurrentPeriodEnd, subscription.CustomerId, subscription.Items.Data[0].Plan.Amount, stripeCustomer.DefaultSourceId);
        await SaveNewSubscription(sqlSubscription);
    }

    public async Task ManageSubscriptionCanceled(User userFromDb)
    {
        await stripeService.CancelSubscription(userFromDb.Subscription.StipeSubscriptionId);
        //await stripeService.DeleteCard(userFromDb.Subscription.StripeCusomerId, userFromDb.Subscription.StripePaymentCardId);

        var deleteSubscriptionCommand = new DeleteSubscriptionCommand() { Parameter = userFromDb.Subscription };
        await commandExecutor.ExecuteCommand(deleteSubscriptionCommand);
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
            Metadata = new Dictionary<string, string> { { "userId", userFromDb.Id.ToString() } },
        };
        var stripeCustomer = await stripeService.CreateStripeCustomerCustomer(createStripeCustomerRequest);

        return stripeCustomer;
    }

    private static DataAccess.Entities.Subscription CreateNewSubscription(int userId, string subscriptionId, DateTime currentPeriodStart, DateTime currentPeriodEnd, string customerId, long? amountPaid, string defaultSourceId)
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
            StripePaymentCardId = defaultSourceId,
        };
    }

    private async Task SaveNewSubscription(DataAccess.Entities.Subscription subscription)
    {
        await commandExecutor.ExecuteCommand(new AddSubscriptionCommand() { Parameter = subscription });
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
