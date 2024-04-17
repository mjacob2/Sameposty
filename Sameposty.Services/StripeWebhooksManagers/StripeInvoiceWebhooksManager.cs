using Hangfire;
using Sameposty.DataAccess.Commands.Invoices;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.Fakturownia;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.StripeServices;

namespace Sameposty.Services.StripeWebhooksManagers;
public class StripeInvoiceWebhooksManager(IQueryExecutor queryExecutor, IEmailService email, IFakturowniaService fakturowniaService, IConfigurator configurator, IPostsGenerator postsGenerator, ICommandExecutor commandExecutor, IStripeService stripeService) : IStripeWebhooksManager
{
    [AutomaticRetry(Attempts = 0)]
    public async Task ManageInvoicePaymentFailed(string userEmail)
    {
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByEmailQuery(userEmail));
        await email.SendNotifyUserPaymentFailedEmail(userFromDb.Email);
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task ManageInvoicePaid(string userEmail)
    {
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByEmailQuery(userEmail));
        var createInvoiceRequest = new AddFakturowniaInvoiceModel(userFromDb.FakturowniaClientId);
        var invoiceCreated = await fakturowniaService.CreateInvoiceAsync(createInvoiceRequest);
        await SaveInvoice(invoiceCreated, userFromDb.Id);
        await fakturowniaService.SendInvoiceToUser(invoiceCreated.Id);

        var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
        var newPostsGenerated = await postsGenerator.GeneratePostsAsync(generatePostRequest, configurator.NumberPremiumPostsGenerated);

        await UpdateUser(userFromDb, newPostsGenerated);
        await email.EmailUserNewPostsGenerated(userFromDb.Email);
    }

    private async Task SaveInvoice(FakturowniaInvoice fakturowniaInvoice, int userId)
    {
        var invoice = new Invoice()
        {
            Number = fakturowniaInvoice.Number,
            IssueDate = fakturowniaInvoice.IssueDate,
            PriceNet = fakturowniaInvoice.PriceNet,
            PriceGross = fakturowniaInvoice.PriceGross,
            Currency = fakturowniaInvoice.Currency,
            FakturowniaClientId = fakturowniaInvoice.FakturowniaClientId,
            UserId = userId,
        };

        var addInvoiceCommand = new AddInvoiceCommand() { Parameter = invoice };
        await commandExecutor.ExecuteCommand(addInvoiceCommand);
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

        if (userFromDb.Role != Roles.Admin)
        {
            userFromDb.ImageTokensUsed += configurator.NumberFirstPostsGenerated;
            userFromDb.TextTokensUsed += configurator.NumberFirstPostsGenerated;
            userFromDb.Role = Roles.PaidUser;
        }

        var updateUserCommand = new UpdateUserCommand() { Parameter = userFromDb };
        await commandExecutor.ExecuteCommand(updateUserCommand);
    }

}
