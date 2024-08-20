using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.REGON;

namespace Sameposty.API.Endpoints.Users.UpdateNip;

public class AddNipEndpoint(ICommandExecutor commandExecutor, IRegonService regonService) : Endpoint<AddNipRequest>
{
    public override void Configure()
    {
        Patch("user/addNIP");
    }

    public override async Task HandleAsync(AddNipRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);

        var regonCompany = await regonService.GetCompanyData(req.Nip);

        if (regonCompany.Nazwa == null || regonCompany.Miejscowosc == null || regonCompany.KodPocztowy == null || regonCompany.Nip == null || !string.IsNullOrEmpty(regonCompany.DataZakonczeniaDzialalnosci))
        {
            ThrowError($"Dane firmy znalezione w bazie REGON są niekompletne: Nazwa:{regonCompany.Nazwa ?? "brak"}, Miejscowość:{regonCompany.Miejscowosc ?? "brak"}, Poczta:{regonCompany.KodPocztowy ?? "brak"}, NIP: {regonCompany.Nip ?? "brak"}, Data zamknięcia: {regonCompany.DataZakonczeniaDzialalnosci}");
        }

        var updateNipCommand = new UpdateUserCompanyInformationsCommand(id, req.Nip, regonCompany.Nazwa, regonCompany.Miejscowosc, regonCompany.KodPocztowy, regonCompany.Ulica, regonCompany.NrNieruchomosci, regonCompany.NrLokalu, regonCompany.Regon);
        var updatedUser = await commandExecutor.ExecuteCommand(updateNipCommand);

        await SendOkAsync("updated", ct);
    }
}
