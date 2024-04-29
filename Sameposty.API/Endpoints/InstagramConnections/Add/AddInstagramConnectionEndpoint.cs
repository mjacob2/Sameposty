using FastEndpoints;
using Sameposty.API.Endpoints.FacebookConnections.Add;
using Sameposty.DataAccess.Commands.InstagramConenctions;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.InstagramConnections;
using Sameposty.Services.FacebookTokenManager;

namespace Sameposty.API.Endpoints.InstagramConnections.Add;

public class AddInstagramConnectionEndpoint(ICommandExecutor commandExecutor, IFacebookTokenManager facebookTokenManager, IQueryExecutor queryExecutor) : Endpoint<AddInstagramConnectionRequest>
{
    public override void Configure()
    {
        Post("instagramConnection");
    }

    public override async Task HandleAsync(AddInstagramConnectionRequest req, CancellationToken ct)
    {
        var instagramConnection = await queryExecutor.ExecuteQuery(new GetInstagramConnectionByPageIdQuery(req.PageId));

        if (instagramConnection != null)
        {
            ThrowError("Ta strona została już dodana na innym koncie, innego użytkownika sameposty");
        }

        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var longLivedUserToken = await facebookTokenManager.GetLongLivedUserAccessToken(req.ShortLivedUserToken);

        var instagramConnectionToAdd = new InstagramConnection()
        {
            AccesToken = longLivedUserToken,
            PageId = req.PageId,
            PageName = req.PageName,
            UserId = userId,
        };

        var newInstagramConnection = await commandExecutor.ExecuteCommand(new AddInstagramConnectionCommand() { Parameter = instagramConnectionToAdd });

        var response = new Response()
        {
            HasValidAccesToken = true,
            Id = newInstagramConnection.Id,
            PageId = newInstagramConnection.PageId,
            PageName = newInstagramConnection.PageName,
        };

        await SendOkAsync(response, ct);
    }
}
