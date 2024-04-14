namespace Sameposty.Services.Fakturownia;
public interface IFakturowniaService
{
    Task<long> CreateClientAsync(AddFakturowniaClientModel clientData);
}
