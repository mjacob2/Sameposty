using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class Subscription : EntityBase
{
    public string CustomerEmail { get; set; } = string.Empty;

    public int OrderId { get; set; }

    public double AmountPaid { get; set; }

    public string SubscriptionCurrentPeriodStart { get; set; } = string.Empty;

    public string SubscriptionCurrentPeriodEnd { get; set; } = string.Empty;

    public string CardTokenId { get; set; } = string.Empty;

    public string StripeCusomerId { get; set; } = string.Empty;

    public string StipeSubscriptionId { get; set; } = string.Empty;

    public string CardLastFourDigits { get; set; } = string.Empty;

    [JsonIgnore]
    public User? User { get; set; }
    public int? UserId { get; set; }
}
