namespace Sameposty.Services.StripeWebhooksManagers;
public interface IStripeWebhooksManager
{
    Task ManageInvoicePaymentFailed(string userEmail);
    Task ManageInvoicePaid(string userEmail);
}
