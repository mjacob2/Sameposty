using Sameposty.Services.Fakturownia;

namespace Sameposty.Services.Tests.FakturowniaTests;
public class FakturowniaTest
{
    [Fact]
    public async Task CreateNewClient()
    {
        // Arrange
        var fakturowniaService = new FakturowniaService("txN5wuxiAaMPDfjhMoj", new HttpClient());


        var request = new AddFakturowniaClientModel()
        {
            City = "JAkeiś miasto",
            Email = "jakis@email.com",
            Name = "Firma json",
            NIP = "4322218540",
            PostCode = "44-111",
            Street = "Jabłonkowa 3",
        };

        // Act
        var tt = await fakturowniaService.CreateClientAsync(request);
    }

    [Fact]
    public async Task CreateNewInvoice()
    {
        var fakturowniaService = new FakturowniaService("txN5wuxiAaMPDfjhMoj", new HttpClient());

        var request = new AddFakturowniaInvoiceModel(144418256);

        var gf = await fakturowniaService.CreateInvoiceAsync(request);
    }

    [Fact]
    public async Task SendInvoiceEmail()
    {
        var fakturowniaService = new FakturowniaService("txN5wuxiAaMPDfjhMoj", new HttpClient());
        await fakturowniaService.SendInvoiceToUser(284935497);
    }
}
