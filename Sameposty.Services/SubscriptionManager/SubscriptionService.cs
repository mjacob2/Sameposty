using Sameposty.DataAccess.Commands.Subscriptions;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.ConfiguratorService;
using Sameposty.Services.Stripe;
using Sameposty.Services.StripeServices;

namespace Sameposty.Services.SubscriptionManager;
public class SubscriptionService(IStripeService stripeService, ICommandExecutor commandExecutor, IConfigurator configurator) : ISubscriptionManager
{
    public async Task ManageSubscriptionCreated(User userFromDb)
    {
        if (userFromDb.Subscription != null)
        {
            throw new ArgumentException("Ten klient już ma subskrypcję!");
        }

        var stripeCustomer = await GetStripeCustomerId(userFromDb);
        var subscription = await stripeService.CreateSubscription(stripeCustomer, userFromDb.Id.ToString());
        var sqlSubscription = CreateNewSubscription(userFromDb.Id, subscription.Id, subscription.CurrentPeriodStart, subscription.CurrentPeriodEnd, subscription.Items.Data[0].Plan.Amount);
        await SaveNewSubscription(sqlSubscription);
    }

    public async Task ManageSubscriptionCanceled(User userFromDb)
    {
        await stripeService.CancelSubscription(userFromDb.Subscription.StipeSubscriptionId);
        //await stripeService.DeleteCard(userFromDb.Subscription.StripeCusomerId, userFromDb.Subscription.StripePaymentCardId);

        var deleteSubscriptionCommand = new DeleteSubscriptionCommand() { Parameter = userFromDb.Subscription };
        await commandExecutor.ExecuteCommand(deleteSubscriptionCommand);
    }

    public async Task<string> GetStripeCustomerId(User userFromDb)
    {
        if (userFromDb.Subscription != null)
        {
            return userFromDb.Subscription.StripeCustomerId;
        }
        else
        {
            var createStripeCustomerRequest = new CreateStripeCustomerRequest()
            {
                City = userFromDb.City,
                Email = userFromDb.Email,
                Name = userFromDb.Name,
                NIP = userFromDb.NIP,
                PostalCode = userFromDb.PostCode,
                Street = userFromDb.Street,
                Metadata = new Dictionary<string, string> { { "userId", userFromDb.Id.ToString() } },
            };

            var stripeCustomer = await stripeService.CreateStripeCustomer(createStripeCustomerRequest);
            userFromDb.Subscription.StripeCustomerId = stripeCustomer.Id;
            await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
            return stripeCustomer.Id;
        }
    }

    private static Subscription CreateNewSubscription(int userId, string subscriptionId, DateTime currentPeriodStart, DateTime currentPeriodEnd, long? amountPaid)
    {
        return new Subscription()
        {
            LastAmountPaid = amountPaid / 100 ?? 0,
            CreatedDate = DateTime.Now,
            UserId = userId,
            SubscriptionCurrentPeriodEnd = currentPeriodEnd.ToString(),
            SubscriptionCurrentPeriodStart = currentPeriodStart.ToString(),
            StipeSubscriptionId = subscriptionId,
        };
    }

    private async Task SaveNewSubscription(Subscription subscription)
    {
        await commandExecutor.ExecuteCommand(new AddSubscriptionCommand() { Parameter = subscription });
    }
}
