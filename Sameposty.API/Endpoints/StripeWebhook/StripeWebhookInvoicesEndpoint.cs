﻿using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.Fakturownia;
using Sameposty.Services.PostsGenerator;
using Sameposty.Services.Stripe;
using Sameposty.Services.SubscriptionManager;
using Stripe;

namespace Sameposty.API.Endpoints.StripeWebhook;

public class StripeWebhookInvoicesEndpoint(IQueryExecutor queryExecutor, IConfigurator configurator, ICommandExecutor commandExecutor, IPostsGenerator postsGenerator, IEmailService email, IStripeService stripeService, ISubscriptionManager subscriptionManager, IFakturowniaService fakturowniaService) : Endpoint<StripeWebhookInvoicesRequest>
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
            var userEmail = req.Data.StripeInvoice.Email;
            var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByEmailQuery(userEmail));

            var generatePostRequest = CreatePostGeneratingRequest(userFromDb);
            var newPostsGenerated = await postsGenerator.GeneratePostsAsync(generatePostRequest, configurator.NumberPremiumPostsGenerated);

            var request = new AddFakturowniaClientModel()
            {
                City = userFromDb.City,
                Email = userFromDb.Email,
                Name = userFromDb.Name,
                NIP = userFromDb.NIP,
                PostCode = userFromDb.PostCode,
                Street = GetStreetNameWithNumbers(userFromDb.Street, userFromDb.BuildingNumber, userFromDb.FlatNumber),
            };

            var fakturowniaClientId = await fakturowniaService.CreateClientAsync(request);
            userFromDb.FakturowniaClientId = fakturowniaClientId;



            await UpdateUser(userFromDb, newPostsGenerated);
            await email.EmailUserNewPostsGenerated(userFromDb.Email);
            // wysłać fakturę do KLienta!

        }
        else if (req.Type == Events.InvoicePaymentFailed)
        {

            var userEmail = req.Data.StripeInvoice.Email;
            var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByEmailQuery(userEmail));
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

    public static string GetStreetNameWithNumbers(string street, string buildingNumber, string? flatNumber)
    {
        var respone = street + " " + buildingNumber;

        if (!string.IsNullOrEmpty(flatNumber))
        {
            respone += $"/{flatNumber}";
        }

        return respone;
    }
}
