using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class Subscription : EntityBase
{
    public double AmountPaid { get; set; }

    public string SubscriptionCurrentPeriodStart { get; set; } = string.Empty;

    public string SubscriptionCurrentPeriodEnd { get; set; } = string.Empty;

    public string StripeCusomerId { get; set; } = string.Empty;

    public string StipeSubscriptionId { get; set; } = string.Empty;

    public string StripePaymentCardId { get; set; } = string.Empty;

    public bool IsCanceled { get; set; }

    public DateTime? CanceledAt { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    public int? UserId { get; set; }
}
