using FastEndpoints;
using Sameposty.API.Endpoints.FacebookConnections.Add;
using Sameposty.DataAccess.Commands.InstagramConenctions;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.FacebookTokenManager;

namespace Sameposty.API.Endpoints.InstagramConnections.Add;

public class AddInstagramConnectionEndpoint(ICommandExecutor commandExecutor, IFacebookTokenManager facebookTokenManager) : Endpoint<AddInstagramConnectionRequest>
{
    public override void Configure()
    {
        Post("instagramConnection");
    }

    public override async Task HandleAsync(AddInstagramConnectionRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var longLivedUserToken = await facebookTokenManager.GetLongLivedUserAccessToken(req.ShortLivedUserToken);

        var instagramConnectionToAdd = new InstagramConnection()
        {
            AccesToken = longLivedUserToken,
            PageId = req.InstagramUserId,
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
