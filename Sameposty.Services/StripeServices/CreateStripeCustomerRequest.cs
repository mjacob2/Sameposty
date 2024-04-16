﻿
namespace Sameposty.Services.Stripe;
public class CreateStripeCustomerRequest
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string CardTokenId { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string PostalCode { get; set; }

    public string NIP { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}
