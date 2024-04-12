namespace Sameposty.Services.REGON;
public interface IRegonService
{
    Task<DanePodmiotu> GetCompanyData(string nip);
}
