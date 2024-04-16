using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class Subscription : EntityBase
{
    public double LastAmountPaid { get; set; }

    public double TotalAmountPaid { get; set; }

    public string SubscriptionCurrentPeriodStart { get; set; } = string.Empty;

    public string SubscriptionCurrentPeriodEnd { get; set; } = string.Empty;

    public string? StipeSubscriptionId { get; set; }

    public string? StripeCustomerId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    public int? UserId { get; set; }
}
