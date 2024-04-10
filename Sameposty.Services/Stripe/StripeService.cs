using Stripe;

namespace Sameposty.Services.Stripe;
public class StripeService
{
    public static async Task CreateACustomer(CreateStripeCustomerRequest req)
    {
        StripeConfiguration.ApiKey = "sk_test_51OjOS6LJdNESLWLIWOKCCgEy7VUlRrW9Ufs8o6smG52WhoFaSLO9SgM0PKPbS8ScwBloaNdfVLBiGbavbYk2r74V00gLxOwkjM";

        var options = new CustomerCreateOptions
        {
            Source = "tok_1P49UCLJdNESLWLIMUZSWoLx",
            Name = req.Name,
            Email = req.Email,
            Shipping = new ShippingOptions()
            {
                Address = new AddressOptions()
                {
                    Country = "Polska",
                    City = req.City,
                    Line1 = req.Street,
                    PostalCode = req.PostalCode
                },
                Name = req.Name,
            },
            TaxIdData = [new CustomerTaxIdDataOptions()
            {
                Type = "eu_vat",
                Value = req.NIP,
            }
            ],
            PaymentMethod = req.PaymentMethod,

        };
        var service = new CustomerService();
        try
        {
            var resposne = await service.CreateAsync(options);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }


    }

    public static async Task CreatePaymentMethod()
    {
        StripeConfiguration.ApiKey = "sk_test_51OjOS6LJdNESLWLIWOKCCgEy7VUlRrW9Ufs8o6smG52WhoFaSLO9SgM0PKPbS8ScwBloaNdfVLBiGbavbYk2r74V00gLxOwkjM";

        var options = new PaymentMethodCreateOptions
        {
            Type = "card",
            Card = new PaymentMethodCardOptions
            {
                Number = "4242424242424242",
                ExpMonth = 8,
                ExpYear = 2026,
                Cvc = "314",
            },
        };
        var service = new PaymentMethodService();

        try
        {
            var response = await service.CreateAsync(options);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }



    }

    public static async Task UpdateCutomer()
    {
        StripeConfiguration.ApiKey = "sk_test_51OjOS6LJdNESLWLIWOKCCgEy7VUlRrW9Ufs8o6smG52WhoFaSLO9SgM0PKPbS8ScwBloaNdfVLBiGbavbYk2r74V00gLxOwkjM";

        var options = new CustomerUpdateOptions
        {
            Metadata = new Dictionary<string, string> { { "order_id", "6735" } },
        };
        var service = new CustomerService();
        service.Update("cus_NffrFeUfNV2Hib", options);
    }
}
