namespace Sameposty.Services.Fakturownia;
public interface IFakturowniaService
{
    Task<long> CreateClientAsync(AddFakturowniaClientModel clientData);
    Task<FakturowniaInvoice> CreateInvoiceAsync(AddFakturowniaInvoiceModel invoiceData);
    Task SendInvoiceToUser(long invoiceId);
}
