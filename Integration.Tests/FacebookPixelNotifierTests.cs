using Sameposty.Services.FacebookPixel;
using Sameposty.Services.Secrets;

namespace Integration.Tests;
public class FacebookPixelNotifierTests : IDisposable
{
    private readonly FacebookPixelNotifier _facebookPixelNotifier;
    private readonly HttpClient _httpClient;

    public FacebookPixelNotifierTests(SecretsProvider secrets)
    {
        _httpClient = new HttpClient();
        _facebookPixelNotifier = new FacebookPixelNotifier(_httpClient, secrets);
    }

    public async Task<string> NotifyNewLeadTest(string useremail)
    {
        Console.WriteLine($"Testing {nameof(_facebookPixelNotifier.NotifyNewPurchaseAsync)}...");

        var result = await _facebookPixelNotifier.NotifyNewPurchaseAsync(useremail);

        Console.WriteLine($"result: {result}");

        return result;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
