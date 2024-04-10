using Sameposty.Services.Stripe;

namespace Sameposty.Services.Tests;
public class StripeTests
{
    [Fact]
    public async Task CreateUserTest()
    {
        var req = new CreateStripeCustomerRequest()
        {
            City = "Wrocław",
            Email = "jakubicki.m@gmail.com",
            Name = "Firma ABC Sp. z o.o.",
            NIP = "PL8971809999",
            PostalCode = "12345",
            Street = "Street 1 /3",
        };

       await StripeService.CreateACustomer(req);
    }

    [Fact]
    public async Task CreatePaymentMethodTest()
    {
        await StripeService.CreatePaymentMethod();
    }
}
