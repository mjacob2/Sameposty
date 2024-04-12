using REGON.Client.Responses;

namespace Sameposty.Services.REGON;
public interface IRegonService
{
    Task<Company> GetCmpanyData(string nip);
}
