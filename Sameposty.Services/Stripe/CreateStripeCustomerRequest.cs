namespace Sameposty.Services.Stripe;
public class CreateStripeCustomerRequest
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string CardTokenId { get; set; }

    public required string City { get; set; }

    public required string Street { get; set; }

    public required string PostalCode { get; set; }

    public required string NIP { get; set; }
}
