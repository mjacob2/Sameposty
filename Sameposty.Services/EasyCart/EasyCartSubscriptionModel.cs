namespace Sameposty.Services.EasyCart;
public class EasyCartSubscriptionModel
{
    public string CustomerEmail { get; set; }

    public int OrderId { get; set; }

    public double AmountPaid { get; set; }

    public bool OrderHasInvoice { get; set; }

    public string SubscriptionCurrentPeriodStart { get; set; }

    public string SubscriptionCurrentPeriodEnd { get; set; }

}
