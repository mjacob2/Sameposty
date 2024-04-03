using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class Subscription : EntityBase
{
    public string CustomerEmail { get; set; }

    public int OrderId { get; set; }

    public double AmountPaid { get; set; }

    public bool OrderHasInvoice { get; set; }

    public string SubscriptionCurrentPeriodStart { get; set; }

    public string SubscriptionCurrentPeriodEnd { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    public int? UserId { get; set; }
}
