using Hangfire;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.ConfiguratorService;
using Sameposty.Services.Email;

namespace Sameposty.Services.StripeWebhooksManagers.Subscriptions;
public class StripeSubscriptionWebhooksManager(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IEmailService email, IConfigurator configurator) : IStripeSubscriptionWebhooksManager
{
    [AutomaticRetry(Attempts = 0)]
    public async Task ManageSubscriptionCreated(Stripe.Subscription subscription)
    {
        var userId = GetUserIdFromStripeSubscriptionEvent(subscription);
        var user = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(userId));

        user.Subscription.StipeSubscriptionId = subscription.Id;
        user.Subscription.SubscriptionCurrentPeriodStart = subscription.CurrentPeriodStart.ToString();
        user.Subscription.SubscriptionCurrentPeriodEnd = subscription.CurrentPeriodEnd.ToString();
        // TODO : Add amount paid to subscription
        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });
        await email.EmailUserSubscriptionCreated(user.Email);
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task ManageSubscriptionDeleted(Stripe.Subscription subscription)
    {
        var userId = GetUserIdFromStripeSubscriptionEvent(subscription);
        var user = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(userId));
        user.Subscription.StipeSubscriptionId = null;

        user.PostsToGenerateLeft = configurator.PostsDefaultLimit;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });
        await email.EmailUserSubscriptionDeleted(user.Email);
    }

    private static int GetUserIdFromStripeSubscriptionEvent(Stripe.Subscription subscription)
    {
        var userId = subscription.Metadata["userId"];
        return string.IsNullOrEmpty(userId) ? throw new ArgumentException("userId is missing in stripe metadata") : int.Parse(userId);
    }
}
