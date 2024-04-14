using System.Text;
using Newtonsoft.Json;

namespace Sameposty.Services.Fakturownia;
public class FakturowniaService(string fakturowniaApiKey, HttpClient http) : IFakturowniaService
{
    private const string clientsUrl = "https://middlers.fakturownia.pl/clients.json";
    private const string invoicesUrl = "https://middlers.fakturownia.pl/invoices.json";

    public async Task<long> CreateClientAsync(AddFakturowniaClientModel clientData)
    {
        var request = new AddFakturowniaClientRequest()
        {
            ApiToken = fakturowniaApiKey,
            Client = clientData,
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await http.PostAsync(clientsUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var fakturowniaClient = JsonConvert.DeserializeObject<FakturowniaClient>(responseString);
            return fakturowniaClient.Id;
        }
        else
        {
            return 0;
        }
    }

    public async Task<long> CreateInvoiceAsync(AddFakturowniaInvoiceModel invoiceData)
    {
        var request = new AddFakturowniaInvoiceRequest()
        {
            ApiToken = fakturowniaApiKey,
            Invoice = invoiceData,
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await http.PostAsync(invoicesUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            //var fakturowniaInvoice = JsonConvert.DeserializeObject<FakturowniaInvoice>(responseString);
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
