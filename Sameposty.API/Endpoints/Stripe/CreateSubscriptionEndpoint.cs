using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Stripe;

namespace Sameposty.API.Endpoints.Stripe;

public class CreateSubscriptionEndpoint(IQueryExecutor queryExecutor, IStripeService stripeService) : Endpoint<CreateSubscriptionRequest>
{
    public override void Configure()
    {
        Post("addSubscription");
    }

    public override async Task HandleAsync(CreateSubscriptionRequest req, CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);
        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(loggedUserId));

        var createStripeCustomerRequest = new CreateStripeCustomerRequest()
        {
            CardTokenId = req.CardTokenId,
            City = userFromDb.City,
            Email = userFromDb.Email,
            Name = userFromDb.Name,
            NIP = userFromDb.NIP,
            PostalCode = userFromDb.PostCode,
            Street = userFromDb.Street,
        };

        try
        {
            var stripeUserId = await stripeService.CreateStripeCustomerCustomer(createStripeCustomerRequest);
            var subscriptionId = await stripeService.CreateSubscription(stripeUserId);

            await SendOkAsync(stripeUserId, ct);
        }
        catch (Exception ex)
        {
            ThrowError(ex.Message);
        }
    }
}
