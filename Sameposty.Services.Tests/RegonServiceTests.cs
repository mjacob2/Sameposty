using Sameposty.Services.REGON;

namespace Sameposty.Services.Tests;
public class RegonServiceTests
{
    private readonly RegonService _regonService;

    private const string apiKey = "c1983509b95e445cb350";

    public RegonServiceTests()
    {
        _regonService = new RegonService();
    }

    [Fact]
    public async Task GetCompanyDataByNipWorks()
    {
        var nip = "8971803272";
        var data = await _regonService.GetCompanyData(nip);

    }
}
