using FastEndpoints;
using Sameposty.DataAccess.Commands.BasicInformations.UpdateById;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.BasicInformations;

namespace Sameposty.API.Endpoints.BasicInformations.UpdateById;

public class UpdateBasicInformationByIdEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : Endpoint<UpdateBasicInformationByIdRequest>
{
    public override void Configure()
    {
        Put("companyInformations");
    }

    public override async Task HandleAsync(UpdateBasicInformationByIdRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var id = int.Parse(loggedUserId);
        var getBasicInformationFromDbQuery = new GetBasicInformationByIdQuery() { Id = req.Id };
        var basicInformationFromDb = await queryExecutor.ExecuteQuery(getBasicInformationFromDbQuery);

        if (basicInformationFromDb == null)
        {
            ThrowError("Brak informacji o firmie");
        }

        if (basicInformationFromDb.UserId != id)
        {
            ThrowError("Nie masz uprawnienień do aktualizacji tego zasobu");
        }

        basicInformationFromDb.BrandName = req.BrandName;
        basicInformationFromDb.Audience = req.Audience;
        basicInformationFromDb.Mission = req.Mission;
        basicInformationFromDb.ProductsAndServices = req.ProductsAndServices;
        basicInformationFromDb.Goals = req.Goals;
        basicInformationFromDb.Assets = req.Assets;

        var updateCommand = new UpdateBasicInformationByIdCommand { Parameter = basicInformationFromDb };

        await commandExecutor.ExecuteCommand(updateCommand);

        await SendOkAsync(basicInformationFromDb, ct);
    }
}
