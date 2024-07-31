using System.Text;
using System.Text.Json;
using Sameposty.Services.Secrets;


namespace Sameposty.Services.Fakturownia;
public class FakturowniaService(ISecretsProvider secrets, HttpClient http) : IFakturowniaService
{
    private const string clientsUrl = "https://middlers.fakturownia.pl/clients.json";
    private const string invoicesUrl = "https://middlers.fakturownia.pl/invoices.json";

    public async Task<long> CreateClientAsync(AddFakturowniaClientModel clientData)
    {
        var request = new AddFakturowniaClientRequest()
        {
            ApiToken = secrets.FakturowniaApiKey,
            Client = clientData,
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await http.PostAsync(clientsUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var fakturowniaClient = JsonSerializer.Deserialize<FakturowniaClient>(responseString);
            return fakturowniaClient.Id;
        }
        else
        {
            return 0;
        }
    }

    public async Task<FakturowniaInvoice> CreateInvoiceAsync(AddFakturowniaInvoiceModel invoiceData)
    {
        var request = new AddFakturowniaInvoiceRequest()
        {
            ApiToken = secrets.FakturowniaApiKey,
            Invoice = invoiceData,
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await http.PostAsync(invoicesUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var fakturowniaInvoice = JsonSerializer.Deserialize<FakturowniaInvoice>(responseString);
            return fakturowniaInvoice;
        }
        else
        {
            throw new HttpRequestException("Fakturownia invoice creation failed");
        }
    }

    public async Task SendInvoiceToUser(long invoiceId)
    {
        var url = $"https://middlers.fakturownia.pl/invoices/{invoiceId}/send_by_email.json?api_token={secrets.FakturowniaApiKey}";

        var response = await http.PostAsync(url, null);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Fakturownia invoice sending failed");
        }
    }
}
