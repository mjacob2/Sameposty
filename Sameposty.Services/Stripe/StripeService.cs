﻿using Stripe;

namespace Sameposty.Services.Stripe;
public class StripeService : IStripeService
{
    private static readonly string StripeApiKey = "sk_test_51OjOS6LJdNESLWLIWOKCCgEy7VUlRrW9Ufs8o6smG52WhoFaSLO9SgM0PKPbS8ScwBloaNdfVLBiGbavbYk2r74V00gLxOwkjM";
    public async Task<Subscription> CreateSubscription(string stripeCustomerId)
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var options = new SubscriptionCreateOptions
        {
            Customer = stripeCustomerId,
            Items =
            [
                new SubscriptionItemOptions() { Price = "price_1P3iW6LJdNESLWLI7IJLtxNq" },
            ],
        };
        var service = new SubscriptionService();
        var result = await service.CreateAsync(options);
        return result;
    }

    public async Task<Customer> CreateStripeCustomerCustomer(CreateStripeCustomerRequest req)
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var options = new CustomerCreateOptions
        {
            Source = req.CardTokenId, // token do karty. czy moze być wykorzystywany wielokrotnie?
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
                Value = "PL" + req.NIP,
            }
            ],

        };
        var service = new CustomerService();

        var resposne = await service.CreateAsync(options);
        return resposne;
    }

    public static async Task UpdateCutomer()
    {
        StripeConfiguration.ApiKey = StripeApiKey;

        var options = new CustomerUpdateOptions
        {
            Metadata = new Dictionary<string, string> { { "order_id", "6735" } },
        };
        var service = new CustomerService();
        service.Update("cus_NffrFeUfNV2Hib", options);
    }
}
