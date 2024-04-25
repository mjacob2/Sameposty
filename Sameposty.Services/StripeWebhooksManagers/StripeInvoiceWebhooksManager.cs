﻿using Hangfire;
using Sameposty.DataAccess.Commands.Invoices;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.Fakturownia;
using Sameposty.Services.PostGeneratingManager;

namespace Sameposty.Services.StripeWebhooksManagers;
public class StripeInvoiceWebhooksManager(IPostGeneratingManager manager, IQueryExecutor queryExecutor, IEmailService email, IFakturowniaService fakturowniaService, IConfigurator configurator, ICommandExecutor commandExecutor) : IStripeWebhooksManager
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
        await UpdateUserTokens(userFromDb);
        var createInvoiceRequest = new AddFakturowniaInvoiceModel(userFromDb.FakturowniaClientId);
        var invoiceCreated = await fakturowniaService.CreateInvoiceAsync(createInvoiceRequest);
        await SaveInvoice(invoiceCreated, userFromDb.Id);
        await fakturowniaService.SendInvoiceToUser(invoiceCreated.Id);
        await manager.ManageGeneratingPosts(userFromDb, configurator.NumberPremiumPostsGenerated);
        await email.EmailUserNewPostsGenerated(userFromDb.Email);
    }

    private async Task UpdateUserTokens(User user)
    {
        user.ImageTokensLimit = configurator.ImageTokensPremiumLimit;
        user.TextTokensLimit = configurator.TextTokensPremiumLimit;
        user.ImageTokensUsed = 0;
        user.TextTokensUsed = 0;

        await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = user });
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
}
