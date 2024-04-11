namespace Sameposty.API.Endpoints.Users.AboutMe;

public class AboutMeResponse
{
    public int Id { get; set; }
    public string Email { get; set; }

    public string NIP { get; set; }

    public string Name { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string BuildingNumber { get; set; }

    public string FlatNumber { get; set; }

    public string PostCode { get; set; }

    public string REGON { get; set; }

    public bool IsSuspended { get; set; }

    public bool IsActive { get; set; }

    public string Role { get; set; }

    public bool IsVerified { get; set; }

    public bool CanGenerateInitialPosts { get; set; }

    public bool CanGenerateImageAI { get; set; }

    public bool CanEditImageAI { get; set; }

    public bool CanGenerateTextAI { get; set; }

    public int ImageTokensLimit { get; set; }

    public int ImageTokensUsed { get; set; }

    public int TextTokensLimit { get; set; }

    public int TextTokensUsed { get; set; }

    public AboutMeResponseSubscription? Subscription { get; set; }
}

public class AboutMeResponseSubscription
{
    public string CustomerEmail { get; set; }

    public int OrderId { get; set; }

    public double AmountPaid { get; set; }

    public bool OrderHasInvoice { get; set; }

    public string SubscriptionCurrentPeriodStart { get; set; }

    public string SubscriptionCurrentPeriodEnd { get; set; }
}
