using Hangfire;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Email;
using Stripe;

namespace Sameposty.Services.StripeWebhooksManagers.Subscriptions;
public class StripeSubscriptionWebhooksManager(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, IEmailService email) : IStripeSubscriptionWebhooksManager
{
    [AutomaticRetry(Attempts = 0)]
    public async Task ManageSubscriptionCreated(Subscription subscription)
    {
        var userId = GetUserIdFromStripeSubscriptionEvent(subscription);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(userId));

        userFromDb.Subscription.StipeSubscriptionId = subscription.Id;
        userFromDb.Subscription.SubscriptionCurrentPeriodStart = subscription.CurrentPeriodStart.ToString();
        userFromDb.Subscription.SubscriptionCurrentPeriodEnd = subscription.CurrentPeriodEnd.ToString();
        // TODO : Add amount paid to subscription
        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
        await email.EmailUserSubscriptionCreated(userFromDb.Email);
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task ManageSubscriptionDeleted(Subscription subscription)
    {
        var userId = GetUserIdFromStripeSubscriptionEvent(subscription);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(userId));
        userFromDb.Subscription.StipeSubscriptionId = null;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
        await email.EmailUserSubscriptionDeleted(userFromDb.Email);
    }

    private static int GetUserIdFromStripeSubscriptionEvent(Subscription subscription)
    {
        var userId = subscription.Metadata["userId"];
        return string.IsNullOrEmpty(userId) ? throw new ArgumentException("userId is missing in stripe metadata") : int.Parse(userId);
    }
}
