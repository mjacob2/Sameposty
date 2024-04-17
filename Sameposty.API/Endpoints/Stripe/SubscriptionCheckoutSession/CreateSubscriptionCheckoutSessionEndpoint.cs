using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Fakturownia;
using Sameposty.Services.Stripe;
using Sameposty.Services.StripeServices;
using Sameposty.Services.SubscriptionManager;

namespace Sameposty.API.Endpoints.Stripe.SubscriptionCheckoutSession;

public class CreateSubscriptionCheckoutSessionEndpoint(IQueryExecutor queryExecutor, IStripeService stripe, ISubscriptionManager manager, IStripeService stripeService, ICommandExecutor commandExecutor, IFakturowniaService fakturowniaService) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("create-checkout-session");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(loggedUserId));

        if (userFromDb.Subscription.StripeCustomerId == null)
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

            try
            {
                var stripeCustomer = await stripeService.CreateStripeCustomer(createStripeCustomerRequest);
                userFromDb.Subscription.StripeCustomerId = stripeCustomer.Id;
                await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
            }
            catch (Exception e)
            {
                ThrowError(e.Message);
            }

        }

        if (userFromDb.FakturowniaClientId == null)
        {
            var request = new AddFakturowniaClientModel()
            {
                City = userFromDb.City,
                Email = userFromDb.Email,
                Name = userFromDb.Name,
                NIP = userFromDb.NIP,
                PostCode = userFromDb.PostCode,
                Street = GetStreetNameWithNumbers(userFromDb.Street, userFromDb.BuildingNumber, userFromDb.FlatNumber),
            };

            try
            {
                var fakturowniaClientId = await fakturowniaService.CreateClientAsync(request);
                userFromDb.FakturowniaClientId = fakturowniaClientId;
                await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userFromDb });
            }
            catch (Exception e)
            {
                ThrowError(e.Message);
            }

        }

        var session = await stripe.CreateSubscriptionSession(userFromDb.Subscription.StripeCustomerId, userFromDb.Id);

        await SendOkAsync(session.Url, ct);
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
