using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.Stripe;
using Sameposty.Services.SubscriptionManager;
using Stripe;

namespace Sameposty.API.Endpoints.StripeWebhook;

public class StripeWebhookInvoicesEndpoint(IQueryExecutor queryExecutor, IConfigurator configurator, ICommandExecutor commandExecutor, IPostsGenerator postsGenerator, IEmailService email, IStripeService stripeService, ISubscriptionManager subscriptionManager) : Endpoint<StripeWebhookInvoicesRequest>
{
    public override void Configure()
    {
        Post("stripeWebhookInvoices");
        AllowAnonymous();
    }

    public override async Task HandleAsync(StripeWebhookInvoicesRequest req, CancellationToken ct)
    {
        await SendOkAsync(ct);

        if (req.Type == Events.InvoicePaid)
        {
            var userId = int.Parse(req.Data.StripeInvoice.SubscriptionDetails.Metadata.UserId);
            var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(userId));

            var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
            var newPostsGenerated = await postsGenerator.GeneratePostsAsync(generatePostRequest, configurator.NumberPremiumPostsGenerated);
            await UpdateUser(userFromDb, newPostsGenerated);
            await email.EmailUserNewPostsGenerated(userFromDb.Email);
            // wysłać fakturę do KLienta!

        }
        else if (req.Type == Events.InvoicePaymentFailed)
        {

            var userId = int.Parse(req.Data.StripeInvoice.SubscriptionDetails.Metadata.UserId);
            var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(userId));
            await subscriptionManager.ManageSubscriptionCanceled(userFromDb);
            await email.SendNotifyUserSubscriptionCanceledPaymentFailedEmail(userFromDb.Email);
        }
        else
        {
            Console.WriteLine("Unhandled event type: {0}", req.Type);
        }
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

    private async Task UpdateUser(User userFromDb, List<Post> newPostsGenerated)
    {
        var stripeSubscription = await stripeService.GetSubscription(userFromDb.Subscription.StipeSubscriptionId);
        userFromDb.Subscription.SubscriptionCurrentPeriodEnd = stripeSubscription.CurrentPeriodEnd.ToString();

        userFromDb.ImageTokensLimit = configurator.ImageTokensPremiumLimit;
        userFromDb.TextTokensLimit = configurator.TextTokensPremiumLimit;

        var currentPosts = userFromDb.Posts;
        currentPosts.AddRange(newPostsGenerated);

        userFromDb.Posts = currentPosts;

        if (userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            userFromDb.ImageTokensUsed += configurator.NumberFirstPostsGenerated;
            userFromDb.TextTokensUsed += configurator.NumberFirstPostsGenerated;
            userFromDb.Role = DataAccess.Entities.Roles.PaidUser;
        }

        var updateUserCommand = new UpdateUserCommand() { Parameter = userFromDb };
        await commandExecutor.ExecuteCommand(updateUserCommand);
    }
}
