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
            Name = "Name",
            NIP = "4441233232",
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

        var request = new AddFakturowniaInvoiceModel(144245441)
        {
            Positions = new FakturowniaPositions()
        };

        var gf = await fakturowniaService.CreateInvoiceAsync(request);
    }
}
