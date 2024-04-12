using Sameposty.Services.Stripe;

namespace Sameposty.Services.Tests;
public class StripeTests(IStripeService stripeService)
{
    [Fact]
    public async Task CreateUserTest()
    {
        var req = new CreateStripeCustomerRequest()
        {
            City = "Wrocław",
            Email = "jakubicki.m@gmail.com",
            Name = "Firma AAA Sp. z o.o.",
            NIP = "PL8971809999",
            PostalCode = "12345",
            Street = "Street 1 /3",
        };

        await stripeService.CreateStripeCustomerCustomer(req);
    }

    [Fact]
    public async Task CreateSubscriptionTest()
    {
        var testCustomerId = "abc";
        await stripeService.CreateSubscription(testCustomerId);
    }
}
