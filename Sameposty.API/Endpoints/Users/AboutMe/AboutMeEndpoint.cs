using FastEndpoints;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Secrets;

namespace Sameposty.API.Endpoints.Users.AboutMe;

public class AboutMeEndpoint(IQueryExecutor queryExecutor, ISecretsProvider secretsProvider) : EndpointWithoutRequest<AboutMeResponse>
{
    public override void Configure()
    {
        Get("about-me");
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);

        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserAboutMeQuery(loggedUserId));

        var response = new AboutMeResponse()
        {
            Id = userFromDb.Id,
            Email = userFromDb.Email,
            NIP = userFromDb.NIP,
            Role = GetRoleString(userFromDb.Role),
            IsVerified = userFromDb.IsVerified,
            CanEditImageAI = userFromDb.Privilege.CanEditImageAI,
            CanGenerateImageAI = userFromDb.Privilege.CanGenerateImageAI,
            CanGenerateTextAI = userFromDb.Privilege.CanGenerateTextAI,
            ImageTokensLimit = userFromDb.ImageTokensLimit,
            ImageTokensUsed = userFromDb.ImageTokensUsed,
            TextTokensLimit = userFromDb.TextTokensLimit,
            TextTokensUsed = userFromDb.TextTokensUsed,
            Name = userFromDb.Name,
            Street = userFromDb.Street,
            BuildingNumber = userFromDb.BuildingNumber,
            City = userFromDb.City,
            FlatNumber = userFromDb.FlatNumber,
            IsActive = userFromDb.IsActive,
            IsSuspended = userFromDb.IsSuspended,
            PostCode = userFromDb.PostCode,
            REGON = userFromDb.REGON,
        };

        if (userFromDb.Subscription != null)
        {
            response.Subscription = new AboutMeResponseSubscription()
            {
                AmountPaid = userFromDb.Subscription.LastAmountPaid,
                SubscriptionCurrentPeriodStart = userFromDb.Subscription.SubscriptionCurrentPeriodStart,
                SubscriptionCurrentPeriodEnd = userFromDb.Subscription.SubscriptionCurrentPeriodEnd,
                StipeSubscriptionId = userFromDb.Subscription.StipeSubscriptionId,
                StripeCusomerId = userFromDb.Subscription.StripeCustomerId,
                StripeApiKey = secretsProvider.StripeApiKey,
            };
        }

        foreach (var invoice in userFromDb.Invoices)
        {
            response.Invoices.Add(new AboutMeInvoiceResponse()
            {
                Id = invoice.FakturowniaInvoiceId,
                Number = invoice.Number,
                IssueDate = invoice.IssueDate,
            });
        }

        await SendOkAsync(response, ct);
    }

    private static string GetRoleString(Roles role)
    {
        return role switch
        {
            DataAccess.Entities.Roles.FreeUser => "Konto bezpłatne",
            DataAccess.Entities.Roles.PaidUser => "Konto płatne",
            DataAccess.Entities.Roles.Admin => "Administrator",
            _ => throw new ArgumentException("Invalid role"),
        };
    }
}
