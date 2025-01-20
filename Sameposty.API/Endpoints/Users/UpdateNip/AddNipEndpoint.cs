using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.REGON;

namespace Sameposty.API.Endpoints.Users.UpdateNip;

public class AddNipEndpoint(ICommandExecutor commandExecutor, IRegonService regonService, IQueryExecutor queryExecutor) : Endpoint<AddNipRequest>
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

        var userToUpdate = await queryExecutor.ExecuteQuery(new GetUserByIdQuery(id));

        if (userToUpdate == null)
        {
            ThrowError("Nie znaleziono użytkownika");
        }

        userToUpdate.NIP = req.Nip;
        userToUpdate.Name = regonCompany.Nazwa;
        userToUpdate.City = regonCompany.Miejscowosc;
        userToUpdate.PostCode = regonCompany.KodPocztowy;
        userToUpdate.Street = regonCompany.Ulica;
        userToUpdate.BuildingNumber = regonCompany.NrNieruchomosci;
        userToUpdate.FlatNumber = regonCompany.NrLokalu;
        userToUpdate.REGON = regonCompany.Regon;

        var updatedUser = await commandExecutor.ExecuteCommand(new UpdateUserCommand() { Parameter = userToUpdate });

        await SendOkAsync("updated", ct);
    }
}
